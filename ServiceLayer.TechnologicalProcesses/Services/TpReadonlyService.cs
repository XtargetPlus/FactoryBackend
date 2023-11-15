using DatabaseLayer.IDbRequests;
using DB.Model.TechnologicalProcessInfo;
using DB;
using DatabaseLayer.DbRequests.TechnologicalProcessToDb;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Enums;
using Shared.Static;
using Shared.Dto.TechnologicalProcess.Issue.Filters;
using Shared.Dto.TechnologicalProcess.Filters;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Filters;
using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess.Issue.Read;
using BizLayer.Repositories.DetailR;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.TechnologicalProcesses.Services.SubClasses;
using AutoMapper;
using Shared.Dto.TechnologicalProcess.Read;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.Read;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using DB.Model.UserInfo;
using Shared.Dto.Detail;

namespace ServiceLayer.TechnologicalProcesses.Services;

/// <summary>
/// Сервисный слой для обращений к данным только на чтение
/// </summary>
public class TpReadonlyService : ErrorsMapper, ITpReadonlyService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcess> _tpRepository;
    private readonly BaseModelRequests<TechnologicalProcessItem> _tpItemsRepository;
    private readonly BaseModelRequests<EquipmentOperation> _equipmentOperationRepository;
    private readonly IMapper _dataMapper;

    public TpReadonlyService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _tpRepository = new(_context, _dataMapper);
        _tpItemsRepository = new(_context, _dataMapper);
        _equipmentOperationRepository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Получение списка технических процессов
    /// </summary>
    /// <param name="filters">Фильтры для выборки</param>
    /// <returns>Список технических процессов</returns>
    public async Task<List<GetAllReadonlyTechProcessDto>?> GetAllTechProcessesAsync(GetAllReadonlyTechProcessRequestFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId != 0)
            productDetailsId = (await new DetailRepository(this, _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = true
            }, _context) ?? new()).Select(d => d.DetailId).ToList();
        return await new TpRequests(_context, _dataMapper).GetAllAsync(filters, productDetailsId);
    }

    /// <summary>
    /// Получение всех операций на станках конкретной операции технического процесса
    /// </summary>
    /// <param name="techProcessItemId">Id операции технического процесса</param>
    /// <returns>Отсортированные по возрастанию приоритета список операций на станках</returns>
    public async Task<GetAllEquipmentOperationDto> GetAllEquipmentOperationsAsync(int techProcessItemId)
    {
        var result = new GetAllEquipmentOperationDto
        {
            TechProcessInfo = await TechProcessItemSimpleRead.GetAsync(_context, techProcessItemId, _dataMapper, this),
            EquipmentOperations = await _equipmentOperationRepository.GetAllAsync<GetEquipmentOperationDto>(
                    filter: eo => eo.TechnologicalProcessItemId == techProcessItemId,
                    orderBy: o => o.OrderBy(eo => eo.Priority))
        };

        return result;
    }

    /// <summary>
    /// Получаем список операций технического процесса
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Отсортированные по возрастанию приоритета операции технического процесса</returns>
    public async Task<List<GetTechProcessItemDto>?> GetAllTpItemsAsync(GetAllTechProcessItemsFilters filters) =>
        await _tpItemsRepository.GetAllAsync<GetTechProcessItemDto>(
            filter: tpi => tpi.TechnologicalProcessId == filters.TechProcessId 
                           && (filters.Priority == 5 ? tpi.Priority % filters.Priority == 0 : tpi.Priority == filters.Priority),
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            addQueryFilters: Convert.ToBoolean((int)filters.VisibilityOptions));

    /// <summary>
    /// Номера веток операции технического процесса
    /// </summary>
    /// <param name="techProcessItemId">Id операции технического процесса</param>
    /// <param name="visibilityOptions">Фильтр включения и выключения глобальных фильтров запроса к базе</param>
    /// <returns>Отсортированный по возрастанию список номеров веток операции технического процесса или null (ошибки)</returns>
    public async Task<List<int>?> GetNumberOfBranchesAsync(int techProcessItemId, QueryFilterOptions visibilityOptions = QueryFilterOptions.TurnOn)
    {
        var branchNumber = await _tpItemsRepository.FindFirstAsync(
            select: tpi => new BranchNumber { TechProcessId = tpi.TechnologicalProcessId, Priority = tpi.Priority }, 
            filter: tpi => tpi.Id == techProcessItemId,
            addQueryFilters: false);

        if (branchNumber is not null)
        {
            var result = await _tpItemsRepository.GetAllAsync(
                filter: tpi => tpi.TechnologicalProcessId == branchNumber.TechProcessId
                               && tpi.MainTechnologicalProcessItemId == techProcessItemId,
                select: tpi => tpi.Priority,
                distinct: true,
                addQueryFilters: Convert.ToBoolean((int)visibilityOptions)) ?? new();

            result.Sort();

            return result;
        }
        
        AddErrors("Не удалось получить информацию о приоритете главной операции тех процесса");
        return null;

    }

    /// <summary>
    /// Получаем подробную информацию о техническом процессе
    /// </summary>
    /// <param name="techProcessId">Id технического процесса</param>
    /// <returns>Подробная информация об техническом процессе</returns>
    public async Task<GetReadonlyTechProcessInfoDto?> GetInfoAsync(int techProcessId) =>
        await _tpRepository.FindFirstAsync<GetReadonlyTechProcessInfoDto>(
            filter: tp => tp.Id == techProcessId,
            include: i => i.Include(tp => tp.TechnologicalProcessStatuses!));

    /// <summary>
    /// Получаем историю разработки технического процесса с применением фильтров 
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список истории разработки технического процесса со статусом и временем, отсортированным по возрастанию времени начала статуса</returns>
    public async Task<List<GeTechProcessDevelopmentStagesDto>?> GetDevelopmentStagesAsync(GetDevelopmentStagesFilters filters) =>
        await new TpStatusesRequest(_context, _dataMapper).GetDevelopmentStagesAsync(filters);

    /// <summary>
    /// Получаем список законченных тех процессов разработчика
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список выполненных тех процессов</returns>
    public async Task<List<GetBaseTechProcessDto>?> GetAllCompletedDeveloperTpsAsync(GetAllCompletedDeveloperTechProcessesFilters filters) =>
        await new TpStatusesRequest(_context, _dataMapper).GetAllCompletedDeveloperTpsAsync(filters);

    /// <summary>
    /// Получаем список тех процессов, готовых к выдаче
    /// </summary>
    /// <returns>Список тех процессов или null (ошибки)</returns>
    public async Task<List<GetExtendedTechProcessDataDto>?> GetAllTpsReadyForDeliveryAsync()
    {
        var results = await _tpRepository.GetAllAsync<GetExtendedTechProcessDataDto>(
            filter: tp => tp.TechnologicalProcessStatuses!.OrderBy(tps => tps.StatusDate).Last().StatusId == (int)TechProcessStatuses.ForIssuance);
        
        if (results is not null) return results.OrderByDescending(tps => tps.Date).ToList();
        
        AddErrors("Не удалось получить список тех процессов готовых к выдаче");
        return null;

    }

    /// <summary>
    /// Получаем список всех выданных тех процессов
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список выданных тех процессов</returns>
    public async Task<List<GetAllIssuedTechProcessesDto>?> GetAllIssuedTechProcessesAsync(GetAllIssuedTechProcessesRequestFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId != 0)
            productDetailsId = (await new DetailRepository(this, _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = true
            }, _context) ?? new()).Select(d => d.DetailId).ToList();
        return await new TpRequests(_context, _dataMapper).GetAllIssuedTechProcessesAsync(filters, productDetailsId);
    }

    /// <summary>
    /// Получаем список всех выданных дубликатов тех процесса
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список выданных дубликатов</returns>
    public async Task<List<GetTechProcessDuplicateDto>?> GetAllIssuedDuplicatesTechProcessAsync(GetAllIssuedDuplicatesTechProcessFilters filters) =>
        await new TpStatusesRequest(_context, _dataMapper).GetAllIssuedDuplicatesTechProcessAsync(filters);

    /// <summary>
    /// Получаем список всех выданных дубликатов тех процессов которые выполнил технолог
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список выдач</returns>
    public async Task<List<IssuedTechProcessesFromTechnologistDto>?> GetAllIssuedTechProcessesFromTechnologistAsync(IssuedTechProcessesFromTechnologistRequestFilters filters, int userId) =>
        await new TpStatusesRequest(_context, _dataMapper).GetAllIssuedTechProcessesFromTechnologistAsync(filters, userId);

    /// <summary>
    /// Получаем словарь возможных замен статуса на другие статусы
    /// </summary>
    /// <param name="statuses">Список статусов, чьи замены нужно узнать</param>
    /// <returns>Словарь, где Key - Id статуса, Value - список Id статусов, на которые может быть заменен текущий статус</returns>
    public Dictionary<int, int[]> GetStatusChangeOptions(List<TechProcessStatuses> statuses)
    {
        Dictionary<int, int[]> result = new();

        foreach (var status in statuses)
        {
            result[(int)status] = status switch
            {
                TechProcessStatuses.NotInWork => TechProcessVariables.ForNotInWork,
                TechProcessStatuses.InWork => TechProcessVariables.ForInWork,
                TechProcessStatuses.Paused => TechProcessVariables.ForPaused,
                TechProcessStatuses.OnApproval => TechProcessVariables.ForOnApproval,
                TechProcessStatuses.ReturnForRework => TechProcessVariables.ForReturnForRework,
                TechProcessStatuses.ForIssuance => TechProcessVariables.ForForIssuance,
                TechProcessStatuses.Issued => TechProcessVariables.ForIssued,
                TechProcessStatuses.Completed => TechProcessVariables.ForCompleted,
                _ => Array.Empty<int>()
            };
        }

        return result;
    }

    /// <summary>
    /// Получаем словарь с доступными статусами конкретным типам 
    /// </summary>
    /// <param name="statusType">Тип статуса</param>
    /// <returns>Словарь, где Key: statusId, Value: название статуса</returns>
    public Dictionary<int, string> GetStatuses(TechProcessStatusType statusType)
    {
        return statusType switch
        {
            TechProcessStatusType.Developer => new()
            {
                { 2, "Не в работе" },
                { 4, "В работе" },
                { 5, "Приостановлен" },
                { 7, "Возврат на доработку" },
                { 11, "Выполнен" },
            },
            TechProcessStatusType.Supervisor => new()
            {
                { 6, "На согласовании" },
            },
            TechProcessStatusType.Archive => new()
            {
                { 8, "На выдачу" },
            },
            _ => new()
            {
                { 2, "Не в работе" },
                { 4, "В работе" },
                { 5, "Приостановлен" },
                { 6, "На согласовании" },
                { 7, "Возврат на доработку" },
                { 11, "Выполнен" },
                { 8, "На выдачу" },
            }
        };
    }

    /// <summary>
    /// Получение списка задач разработчика
    /// </summary>
    /// <param name="developerId">Id разработчика</param>
    /// <returns>Список задач</returns>
    public async Task<List<GetDeveloperTasksDto>?> GetDeveloperTasksAsync(int developerId)
    {
        var tasks = await _tpRepository.GetAllAsync<DeveloperTaskDto>(
            filter: tp => tp.DeveloperId == developerId && tp.DevelopmentPriority > 0,
            orderBy: o => o.OrderBy(tp => tp.DevelopmentPriority));

        var result = new List<GetDeveloperTasksDto>
        {
            new()
            {
                StatusId = (int)TechProcessStatusesForDirector.NotInWork,
                Status = "Не в работе",
                Tasks = tasks?.Where(t => t.StatusId == (int)TechProcessStatusesForDirector.NotInWork).ToList()
            },
            new()
            {
                StatusId = (int)TechProcessStatusesForDeveloper.InWork,
                Status = "В работе",
                Tasks = tasks?.Where(t => t.StatusId == (int)TechProcessStatusesForDeveloper.InWork).ToList()
            },
            new()
            {
                StatusId = (int)TechProcessStatusesForDeveloper.Paused,
                Status = "Приостановлен",
                Tasks = tasks?.Where(t => t.StatusId ==(int) TechProcessStatusesForDeveloper.Paused).ToList()
            },
            new()
            {
                StatusId = (int)TechProcessStatusesForDeveloper.OnApproval,
                Status = "На согласовании",
                Tasks = tasks?.Where(t => t.StatusId ==(int) TechProcessStatusesForDeveloper.OnApproval).ToList()
            },
            new()
            {
                StatusId = (int)TechProcessStatusesForDirector.ReturnForRework,
                Status = "Возврат на доработку",
                Tasks = tasks?.Where(t => t.StatusId ==(int) TechProcessStatusesForDirector.ReturnForRework).ToList()
            },
            new()
            {   
                StatusId = (int)TechProcessStatusesForDirector.Completed,
                Status = "Выполнено",
                Tasks = tasks?.Where(t => t.StatusId ==(int) TechProcessStatusesForDirector.Completed).ToList()
            },
            new()
            {
                StatusId = (int)TechProcessStatusesForDeveloper.ForIssuance,
                Status = "На выдачу",
                Tasks = tasks?.Where(t => t.StatusId ==(int) TechProcessStatusesForDeveloper.ForIssuance).ToList()
            },
        };

        return result;
    }

    public void Dispose() => _context.Dispose();
}
