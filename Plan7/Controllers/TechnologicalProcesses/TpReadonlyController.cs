using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.TechnologicalProcesses;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.Read;
using Shared.Dto.TechnologicalProcess.Filters;
using Shared.Dto.TechnologicalProcess.Issue.Filters;
using Shared.Dto.TechnologicalProcess.Read;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Filters;
using Shared.Enums;
using Shared.Static;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Branch;

namespace Plan7.Controllers.TechnologicalProcesses;

/// <summary>
/// Контроллер для тех процессов только на чтение
/// </summary>
public class TpReadonlyController : BaseReactController<TpReadonlyController>
{
    private readonly ITpReadonlyService _service;
    private readonly ITpCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public TpReadonlyController(ILogger<TpReadonlyController> logger, ITpReadonlyService service, ITpCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Получение списка технических процессов
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список тех процессов с количеством или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllTps)]
    public async Task<IActionResult> GetAllTps([FromQuery] GetAllReadonlyTechProcessRequestFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех технологов", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await _count.GetAllAsync(filters);

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllTechProcessesAsync(filters), Length = count });
    }

    /// <summary>
    /// Список выполненных тех процессов технолога с учетом фильтров
    /// </summary>
    /// <param name="filters"></param>
    /// <returns>Список выполненных тех процессов, отсортированных по дате от самого последнего до самого первого</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllCompletedDeveloperTps)]
    public async Task<IActionResult> GetAllCompletedDeveloperTps([FromQuery] GetAllCompletedDeveloperTechProcessesFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех технологов", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await _count.GetAllCompletedDeveloperTpsAsync(filters.DeveloperId);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllCompletedDeveloperTpsAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение всех операций тех процесса определенной ветки
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список операций</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllTpItems)]
    public async Task<ActionResult<List<GetTechProcessItemDto>>> GetAllTpItems([FromQuery] GetAllTechProcessItemsFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка операций тех процесса {TechProcessId}: приоритет ветки {Priority}",
            HttpContext.User.FindFirstValue("UserId"), filters.TechProcessId, filters.Priority);
        
        return Ok(await _service.GetAllTpItemsAsync(filters));
    }

    /// <summary>
    /// Получение списка операций на станках операции тех процесса
    /// </summary>
    /// <param name="techProcessItemId">Id операции тех процесса</param>
    /// <returns>Список операций на станках</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllTpOperationEquipments)]
    public async Task<ActionResult<GetAllEquipmentOperationDto>> GetAllTpOperationEquipments([FromQuery, Range(1, int.MaxValue)] int techProcessItemId)
    {
        _logger.LogInformation("Пользователь {userId}: Получения списка операций на станках операции {TechProcessItemId}", HttpContext.User.FindFirstValue("UserId"), techProcessItemId);

        var result = await _service.GetAllEquipmentOperationsAsync(techProcessItemId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }

    /// <summary>
    /// Получение хронологии разработки технического процесса
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список этапов разработки</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetDevelopmentStages)]
    public async Task<ActionResult<List<GeTechProcessDevelopmentStagesDto>>> GetDevelopmentStages([FromQuery] GetDevelopmentStagesFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение хронологии разработки тех процесса {TechProcessId}", HttpContext.User.FindFirstValue("UserId"), filters.TechProcessId);
        
        return Ok(await _service.GetDevelopmentStagesAsync(filters));
    }

    /// <summary>
    /// Получение списка веток определенной операции тех процесса 
    /// </summary>
    /// <param name="dto">Id операции тех процесса и какие ветки получать</param>
    /// <returns>Список веток или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetNumberOfBrunches)]
    public async Task<ActionResult<List<GetNumberOfBrunchesResponseDto>>> GetNumberOfBrunches([FromQuery] GetNumberOfBrunchesRequestDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение количества ответвлений {techProcessId}", HttpContext.User.FindFirstValue("UserId"), dto.TechProcessItemId);

        var result = (await _service.GetNumberOfBranchesAsync(dto.TechProcessItemId))
            .Select(branch => new GetNumberOfBrunchesResponseDto { Branch = branch, Visibility = true })
            .ToList();

        if (!dto.Visibility)
        {
            var branches = await _service.GetNumberOfBranchesAsync(dto.TechProcessItemId, QueryFilterOptions.TurnOff);

            foreach (var branch in branches?.Where(branch => result.All(obj => obj.Branch != branch)))
            {
                result.Add(new GetNumberOfBrunchesResponseDto
                {
                    Branch = branch,
                    Visibility = false
                });
            }
        }

        result = result.OrderBy(obj => obj.Branch).ToList();

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(result);
    }

    /// <summary>
    /// Подробная информация тех процесса
    /// </summary>
    /// <param name="techProcessId">Id тех процесса, чью подробную информацию нужно получить</param>
    /// <returns>Информация о тех процессе</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetInfo)]
    public async Task<ActionResult<GetReadonlyTechProcessInfoDto>> GetInfo([FromQuery, Range(1, int.MaxValue)] int techProcessId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение подробной информации о технологии - {TechProcessId}", HttpContext.User.FindFirstValue("UserId"), techProcessId);
        
        return Ok(await _service.GetInfoAsync(techProcessId));
    }

    /// <summary>
    /// Получаем список тех процессов, готовых к выдаче
    /// </summary>
    /// <returns>Список тех процессов, готовых к выдаче или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllTpsReadyForDelivery)]
    public async Task<ActionResult<List<GetExtendedTechProcessDataDto>>> GetAllTpsReadyForDelivery()
    {
        _logger.LogInformation("Пользователь {userId}: Получение тех процессов, готовых к выдаче", HttpContext.User.FindFirstValue("UserId"));
        
        var techProcesses = await _service.GetAllTpsReadyForDeliveryAsync();
        
        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(techProcesses);
    }

    /// <summary>
    /// Получаем список выданных тех процессов
    /// </summary>
    /// <param name="filters">Фильтры запроса</param>
    /// <returns>Список выданных тех процессов и общее количество с учетом фильтров</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllIssuedTechProcesses)]
    public async Task<IActionResult> GetAllIssuedTechProcesses([FromQuery] GetAllIssuedTechProcessesRequestFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка выданных тех процессов", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await _count.GetAllIssuedAsync(filters);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllIssuedTechProcessesAsync(filters), Length = count });
    }

    /// <summary>
    /// Список выданных дубликатов тех процесса
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список выданных дубликатов тех процесса</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllIssuedDuplicatesTechProcess)]
    public async Task<ActionResult<List<GetTechProcessDuplicateDto>>> GetAllIssuedDuplicatesTechProcess([FromQuery] GetAllIssuedDuplicatesTechProcessFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получаем список выданных дубликатов тех процесса {TechProcessId}", HttpContext.User.FindFirstValue("UserId"), filters.TechProcessId);
        
        return Ok(await _service.GetAllIssuedDuplicatesTechProcessAsync(filters));
    }

    /// <summary>
    /// Получаем список выданных дубликатов тех процессов, которые выполнил технолог с учетом переданных фильтров
    /// </summary>
    /// <param name="filters">Фильтры</param>
    /// <returns>Список выданных дубликатов с учетом фильтров и общим количеством всех записей, подходящие под фильтры без учета пагинации</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetAllIssuedTechProcessesFromTechnologist)]
    public async Task<IActionResult> GetAllIssuedTechProcessesFromTechnologist([FromQuery] IssuedTechProcessesFromTechnologistRequestFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получаем список выданных дубликатов тех процессов", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await _count.GetAllIssuedFromTechnologistAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllIssuedTechProcessesFromTechnologistAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")), Length = count });
    }

    /// <summary>
    /// Получаем информацию, на какие статусы могут быть заменены переданные статусы
    /// </summary>
    /// <param name="statuses">Список статусов, чьи заменяемости нужно получить</param>
    /// <returns>Словарь, где Key - переданный статус, Value - массив возможных смен</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetStatusChangeOptions)]
    public ActionResult<Dictionary<int, int[]>> GetStatusChangeOptions([FromQuery, MinLength(1)] List<TechProcessStatuses> statuses)
    {
        _logger.LogInformation("Пользователь {userId}: Получаем информацию, на какие статусы могут меняться переданные статусы", HttpContext.User.FindFirstValue("UserId"));
       
        return Ok(_service.GetStatusChangeOptions(statuses));
    }

    /// <summary>
    /// Получаем статусы разработки конкретного типа, которые редактируются на другие статусы
    /// </summary>
    /// <param name="statusType">0 чтобы получить все типы кроме "Выдать" и "Выдать дубликат", 1 - Developer, 2 - Supervisor, 3 - Archive</param>
    /// <returns>Словарь, Key: statusId, Value: Название статуса</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetStatuses)]
    public ActionResult<Dictionary<int, string>> GetStatuses([FromQuery] TechProcessStatusType statusType)
    {
        _logger.LogInformation("Пользователь {userId}: Получаем статусы разработки конкретного типа {statusType}", HttpContext.User.FindFirstValue("UserId"), statusType);
        
        return Ok(_service.GetStatuses(statusType));
    }

    /// <summary>
    /// Получение списка задач разработчика
    /// </summary>
    /// <returns>Список задач</returns>
    [HttpGet]
    [Authorize(Roles = TpReadonlyControllerRoles.GetDeveloperTasks)]
    public async Task<ActionResult<GetDeveloperTasksDto>> GetDeveloperTasks()
    {
        _logger.LogInformation("Пользователь {userId}: Получение собственных тех процессов в разработке", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetDeveloperTasksAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
    }
}
