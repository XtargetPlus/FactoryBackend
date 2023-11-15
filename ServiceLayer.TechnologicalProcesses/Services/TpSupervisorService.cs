using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.TechnologicalProcessInfo;
using DatabaseLayer.DbRequests.TechnologicalProcessToDb;
using Shared.Dto.TechnologicalProcess;
using Shared.Enums;
using BizLayer.Repositories.TechnologicalProcessR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using BizLayer.Repositories.DetailR;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using ServiceLayer.TechnologicalProcesses.Services.Developer;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Collections.Immutable;
using DatabaseLayer.IDbRequests.UserToDb;
using DB.Model.StatusInfo;
using Shared.Dto.TechnologicalProcess.CUD;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Read;
using Shared.Dto.TechnologicalProcess.Status;
using Shared.Dto.Detail;

namespace ServiceLayer.TechnologicalProcesses.Services;

/// <summary>
/// Сервис для логики директоров технологов
/// </summary>
public class TpSupervisorService : ErrorsMapper, ITpSupervisorService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcess> _repository;
    private readonly IMapper _dataMapper;

    public TpSupervisorService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }
     
    /// <summary>
    /// Добавление технического процесса
    /// </summary>
    /// <param name="dto">Информация об техническом процессе</param>
    /// <returns>Id добавленного тех процесса или null (ошибки с предупреждениями)</returns>
    public async Task<int?> AddAsync(AddTechProcessDto dto)
    {
        if (dto.FinishDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0)
        {
            AddWarnings((new List<ValidationResult>() 
            { 
                new("Запрещено ставить прошедшую дату", new[] { nameof(dto.FinishDate) }) 
            }).ToImmutableList());
            return null;
        }

        // взаимодействия с базой проводятся внутри транзакции
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        SupervisorRepository supervisorRepository = new(_repository, this);

        var techProcess = supervisorRepository.Create(dto);
        techProcess = await _context.AddWithValidationsAndSaveAsync(techProcess, this);
        if (HasWarnings || HasErrors)
            return null;

        if (await supervisorRepository.UpdateHigherDeveloperTasksAsync(dto, techProcess!.Id) > 0)
            await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors || HasWarnings)
            return null;
        
        await transaction.CommitAsync();
        
        return techProcess!.Id;
    }

    /// <summary>
    /// Меняем статус разработки тех процесса
    /// </summary>
    /// <param name="dto">Информация для изменения статуса разработки тех процесса</param>
    /// <returns>1 или null (ошибки с предупреждениями)</returns>
    public async Task ChangeTpStatusAsync(SupervisorChangeTechProcessStatusDto dto, int userId)
    {
        TpDeveloperService developerService = new(_context, _dataMapper);
        // Вызываем базовый методы изменения статуса разработки
        await developerService.ChangeTpStatusAsync(dto, (int)dto.TechProcessStatuses, userId);
        
        if (developerService.HasErrors)
            AddErrors(string.Join("\n", developerService.Errors));
        if (developerService.HasWarnings)
            AddWarnings(developerService.Warnings);
    }

    /// <summary>
    /// Меняем разработчика тех процесса
    /// </summary>
    /// <param name="dto">Информация для изменения разработчика тех процесса</param>
    /// <returns>1 или null (ошибки с предупреждениями)</returns>
    public async Task ChangeProcessDeveloperAsync(ChangeTechProcessDeveloperDto dto)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, this);
        if (HasErrors)
            return;

        await ChangeAbovePriorityDevelopedTechProcesses(techProcess!.DeveloperId, techProcess.DevelopmentPriority, false);
        await ChangeAbovePriorityDevelopedTechProcesses(dto.DeveloperId, dto.DeveloperPriority - 1, true);
        if (HasErrors)
            return;

        techProcess.DeveloperId = dto.DeveloperId;
        techProcess.DevelopmentPriority = dto.DeveloperPriority;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменение приоритетов внутри одного разработчика (вставка между)
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns></returns>
    public async Task ChangeTechProcessPriorityAsync(ChangeTechProcessPriorityDto dto)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, this);
        if (HasErrors)
            return;

        await ChangeRangePriorityTechProcesses(techProcess!.DeveloperId, dto.Priority, techProcess.DevelopmentPriority, techProcess.DevelopmentPriority > dto.Priority);
        if (HasErrors)
            return;

        techProcess.DevelopmentPriority = dto.Priority;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Возврат тех процесса на доработку
    /// </summary>
    /// <param name="dto">Информация для возврата</param>
    /// <param name="userId">Id инициатора смены статуса</param>
    /// <returns></returns>
    public async Task ReturnToWorkAsync(ReturnInWorkDto dto, int userId)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, this);
        if (HasErrors)
            return;

        await ChangeAbovePriorityDevelopedTechProcesses(dto.DeveloperId, dto.DeveloperPriority - 1, true);
        if (HasErrors)
            return;

        techProcess!.DeveloperId = dto.DeveloperId;
        techProcess.DevelopmentPriority = dto.DeveloperPriority;
        techProcess.FinishDate = dto.NewFinishDate;
        techProcess.TechnologicalProcessStatuses = new List<TechnologicalProcessStatus>
        {
            new()
            {
                StatusId = (int)TechProcessStatusesForDirector.ReturnForRework,
                StatusDate = DateTime.Now,
                UserId = userId,
                Note = dto.Note
            }
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.SaveChangesWithValidationsAsync(this);
        await _context.RemoveWithValidationAndSaveAsync(new TechnologicalProcessStatus
        {
            Id = await TechProcessSimpleRead.GetIssuedStatusIdAsync(_context, techProcess.Id)
        }, this);

        if (HasErrors)
            return;

        await transaction.CommitAsync();
    }


    /// <summary>
    /// Удаление технического процесса
    /// </summary>
    /// <param name="techProcessId">Id технического процесса</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int techProcessId)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, techProcessId, this);
        if (HasErrors)
            return;

        await ChangeAbovePriorityDevelopedTechProcesses(techProcess!.DeveloperId, techProcess.DevelopmentPriority, false);
        if (HasErrors)
            return;

        await _context.RemoveWithValidationAndSaveAsync(techProcess, this);
    }

    /// <summary>
    /// Проверяем, есть ли у детали технические процессы
    /// </summary>
    /// <param name="detailId">Id детали</param>
    /// <returns>true или false</returns>
    public async Task<bool> CheckAvailabilityAsync(int detailId) =>
        await _repository.FindFirstAsync(filter: tp => tp.DetailId == detailId) is not null;

    /// <summary>
    /// Проверяем список деталей на наличие технических процессов
    /// </summary>
    /// <param name="detailsId">Список Id деталей</param>
    /// <returns>Словарь где Key - Id детали, Value - true или false</returns>
    public async Task<Dictionary<int, bool>> CheckRangeAvailabilitiesAsync(List<int> detailsId)
    {
        Dictionary<int, bool> checks = new();
        var resultsId = await _repository.GetAllAsync(filter: f => detailsId.Contains(f.DetailId), select: f => f.DetailId);
        if (resultsId is null)
            return checks;
        foreach (var id in detailsId)
        {
            checks.Add(id, false);
            if (resultsId.Contains(id))
                checks[id] = true;
        }
        return checks;
    }

    /// <summary>
    /// Получаем список технических процессов определенной детали
    /// </summary>
    /// <param name="detailId">Id детали</param>
    /// <returns>Список технических процессов</returns>
    public async Task<GetDetailTechProcessesDto?> GetDetailTpsAsync(int detailId) =>
        await new TpRequests(_context, _dataMapper).GetAllDetailTps(detailId);

    /// <summary>
    /// Получаем словарь, где Key - developerId, Value - отсортированные по приоритету текущие задачи
    /// </summary>
    /// <param name="developerIds">Список developerId</param>
    /// <param name="productId"></param>
    /// <returns>Словарь с разработчиками и их задачами</returns>
    public async Task<List<GetAllDevelopersTasksDto>?> GetAllDevelopersTasksAsync(List<int> developerIds, int productId)
    {
        var result = await new UserRequests(_context, _dataMapper).GetAllTechnologists(developerIds);
        if (result is null)
        {
            AddErrors("Не удалось получить список технологов");
            return null;
        }

        SupervisorRepository supervisorRepository = new(_repository, this);

        List<int> productDetailsId = new() { productId };
        if (productId > 0)
            productDetailsId.AddRange((await new DetailRepository(this, _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = productId,
                IsHardDetail = true
            }, _context) ?? new()).Select(d => d.DetailId).ToList());

        foreach (var developer in result)
        {
            var tasks = await supervisorRepository.GetDeveloperTasksAsync(developer.DeveloperId, productDetailsId);
            if (tasks is null)
                return null;
            
            developer.Tasks = tasks;
        }
        return result;
    }

    /// <summary>
    /// Меняет приоритеты разработки всех тех процессов разработчика на +-1, чей приоритет больше передаваемого
    /// </summary>
    /// <param name="developerId">Id разработчика, чьи тех процессы нужно получить</param>
    /// <param name="developerPriority">Приоритет разработки, больше которого тех процессы нам нужны</param>
    /// <param name="enlarge">true - увеличить всем полученным тех процессам приоритет разработки, false - уменьшить</param>
    /// <returns>Ничего или объект Task, если не используется await</returns>
    private async Task ChangeAbovePriorityDevelopedTechProcesses(int developerId, int developerPriority, bool enlarge)
    {
        var developerTechProcess = await _repository.GetAllAsync(
            filter: tp => tp.DeveloperId == developerId && tp.DevelopmentPriority > developerPriority,
            orderBy: o => o.OrderBy(tp => tp.DevelopmentPriority),
            trackingOptions: TrackingOptions.WithTracking);

        if (developerTechProcess is null)
            AddErrors("Не удалось получить разрабатываемые тех процессы с приоритетом выше передаваемого");

        developerTechProcess?.ForEach(tp => tp.DevelopmentPriority += (byte)(enlarge ? 1 : -1));
    }

    /// <summary>
    /// Уменьшение приоритетов тех процессов в определенном диапазоне приоритетов
    /// </summary>
    /// <param name="developerId"></param>
    /// <param name="newPriority"></param>
    /// <param name="oldPriority"></param>
    /// <param name="enlarge"></param>
    /// <returns></returns>
    private async Task ChangeRangePriorityTechProcesses(int developerId, int newPriority, int oldPriority, bool enlarge)
    {
        var developerTechProcess = await _repository.GetAllAsync(
            filter: tp => tp.DeveloperId == developerId && enlarge 
                ? (tp.DevelopmentPriority >= newPriority && tp.DevelopmentPriority < oldPriority) 
                : (tp.DevelopmentPriority <= newPriority && tp.DevelopmentPriority > oldPriority),
            orderBy: o => o.OrderBy(tp => tp.DevelopmentPriority),
            trackingOptions: TrackingOptions.WithTracking);

        if (developerTechProcess is null)
            AddErrors("Не удалось получить разрабатываемые тех процессы с приоритетом выше передаваемого");

        developerTechProcess?.ForEach(tp => tp.DevelopmentPriority += (byte)(enlarge ? 1 : -1));
    }

    public void Dispose() => _context.Dispose();
}
