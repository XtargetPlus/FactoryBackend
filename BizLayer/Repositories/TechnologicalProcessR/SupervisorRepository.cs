using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StatusInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Enums;
using Shared.Static;

namespace BizLayer.Repositories.TechnologicalProcessR;

public class SupervisorRepository
{
    private readonly BaseModelRequests<TechnologicalProcess> _repository;
    private readonly ErrorsMapper _mapper;

    public SupervisorRepository(BaseModelRequests<TechnologicalProcess> repository, ErrorsMapper mapper) => (_mapper, _repository) = (mapper, repository);

    public TechnologicalProcess Create(AddTechProcessDto dto)
    {
        return new()
        {
            DetailId = dto.DetailId,
            ManufacturingPriority = (byte)(_repository.GetAll(filter: tp => tp.DetailId == dto.DetailId)?.OrderBy(tp => tp.ManufacturingPriority).LastOrDefault()?.ManufacturingPriority + 1 ?? 1),
            FinishDate = dto.FinishDate,
            IsActual = true,
            DeveloperId = dto.DeveloperId,
            DevelopmentPriority = dto.DevelopmentPriority,
            TechnologicalProcessData = new TechnologicalProcessData(),
            TechnologicalProcessStatuses = new()
            {
                new TechnologicalProcessStatus
                {
                    StatusId = (int)TechProcessStatuses.NotInWork,
                    Note = dto.Note,
                    StatusDate = DateTime.Now,
                    UserId = dto.DeveloperId
                }
            }
        };
    }

    public async Task<int> UpdateHigherDeveloperTasksAsync(AddTechProcessDto dto, int techProcessId)
    {
        // Получаем список технических процессов, чей приоритет разработки равен или больше чем у созданного и увеличиваем их приоритет на +1
        var techProcesses = await _repository.GetAllAsync(
            filter: tp => tp.Id != techProcessId
                          && tp.DeveloperId == dto.DeveloperId
                          && tp.DevelopmentPriority >= dto.DevelopmentPriority
                          && tp.TechnologicalProcessStatuses!.All(tps => tps.StatusId != (int)TechProcessStatuses.Issued),
            trackingOptions: TrackingOptions.WithTracking);

        if (techProcesses == null)
            _mapper.AddErrors("Не удалось получить список тех процессов с приоритетом разработки выше переданного");
            
        techProcesses?.ForEach(tp => tp.DevelopmentPriority++);
        return techProcesses?.Count ?? 0;
    }

    public async Task<List<GetExtendedTechProcessDataDto>?> GetDeveloperTasksAsync(int developerId, List<int> productDetailsId)
    {
        var tasks = await _repository.GetAllAsync(
            filter: tp => tp.DeveloperId == developerId
                          && tp.DevelopmentPriority > 0
                          && (productDetailsId.First() <= 0 || productDetailsId.Contains(tp.DetailId)),
            orderBy: o => o.OrderBy(tp => tp.DevelopmentPriority),
            select: tp => new GetExtendedTechProcessDataDto()
            {
                TechProcessId = tp.Id,
                SerialNumber = tp.Detail!.SerialNumber,
                Title = tp.Detail.Title,
                Date = tp.FinishDate,
                Priority = tp.DevelopmentPriority,
                Note = tp.TechnologicalProcessStatuses!.OrderBy(tps => tps.StatusDate).Last().Note,
                Status = tp.TechnologicalProcessStatuses!.OrderBy(tps => tps.StatusDate).Last().Status!.Title
            });
        if (tasks is null)
            _mapper.AddErrors("Не удалось получить список тех процессов в работе пользователя");
        return tasks;
    }
}