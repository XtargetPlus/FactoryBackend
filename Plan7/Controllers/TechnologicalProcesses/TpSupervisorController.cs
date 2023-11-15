using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.TechnologicalProcesses;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Read;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Shared.Dto.TechnologicalProcess.CUD;
using Shared.Dto.TechnologicalProcess.Status;

namespace Plan7.Controllers.TechnologicalProcesses;

/// <summary>
/// Контроллер для начальников технологов
/// </summary>
public class TpSupervisorController : BaseReactController<TpSupervisorController>
{
    private readonly ITpSupervisorService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TpSupervisorController(ILogger<TpSupervisorController> logger, ITpSupervisorService service)
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление технического процесса
    /// </summary>
    /// <param name="dto">Информация об тех процессе, где
    /// DetailId - id детали, для которой создается тех процесс; FinishDate - дата окончания разработки;
    /// DevelopmentPriority - приоритет разработки; DeveloperId - id разработчика тех процесса; Note - примечание, возможен null</param>
    /// <returns>Id или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpSupervisorControllerRoles.AddTp)]
    public async Task<IActionResult> AddTp(AddTechProcessDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Проверка детали на наличие техпроцессов", HttpContext.User.FindFirstValue("UserId"));

        var id = await _service.AddAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Смена статуса для директоров технологов
    /// </summary>
    /// <param name="dto">Информация для смены статуса, где
    /// TechProcessId - id тех процесса; Note - примечание, возможен null; TechProcessStatuses - статусы, которые может менять директор технолог</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpSupervisorControllerRoles.ChangeStatus)]
    public async Task<IActionResult> ChangeStatus(SupervisorChangeTechProcessStatusDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Проверка детали на наличие тех процессов", HttpContext.User.FindFirstValue("UserId"));

        await _service.ChangeTpStatusAsync(dto, int.Parse(HttpContext.User.FindFirstValue("UserId")!));
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена разработчика тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены разработчика тех процесса, где
    /// TechProcessId - id тех процесса; DeveloperId - id нового разработчика; DeveloperPriority - новый приоритет</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpSupervisorControllerRoles.ChangeProcessDeveloper)]
    public async Task<IActionResult> ChangeProcessDeveloper(ChangeTechProcessDeveloperDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена разработчика тех процесса {TechProcessId} на {developerId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.DeveloperId);
        
        await _service.ChangeProcessDeveloperAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение приоритетов тех процессов внутри одного разработчика (вставка между)
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpSupervisorControllerRoles.ChangeTechProcessPriority)]
    public async Task<IActionResult> ChangeTechProcessPriority(ChangeTechProcessPriorityDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: смена приоритета тех процесса {techProcessId} на {priority}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.Priority);

        await _service.ChangeTechProcessPriorityAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Возврат тех процесса на доработку
    /// </summary>
    /// <param name="dto">Информация для возврата</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = TpSupervisorControllerRoles.ReturnToWork)]
    public async Task<IActionResult> ReturnToWork(ReturnInWorkDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: возврат тех процесса {techProcessId} на доработку пользователю {developerId} с приориетом {priority}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.DeveloperId, dto.DeveloperPriority);

        await _service.ReturnToWorkAsync(dto, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление технического процесса
    /// </summary>
    /// <param name="techProcessId">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{techProcessId:int}")]
    [Authorize(Roles = TpSupervisorControllerRoles.DeleteTp)]
    public async Task<IActionResult> DeleteTp(int techProcessId)
    {
        _logger.LogInformation("Пользователь {userID}: Удаление тех процесса {techProcessId}", HttpContext.User.FindFirstValue("UserId"), techProcessId);
        
        await _service.DeleteAsync(techProcessId);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Проверка, есть ли у детали тех процесс
    /// </summary>
    /// <param name="detailId">Id детали</param>
    /// <returns>true или false</returns>
    [HttpGet]
    [Authorize(Roles = TpSupervisorControllerRoles.CheckAvailability)]
    public async Task<IActionResult> CheckAvailability([FromQuery, Range(1, int.MaxValue)] int detailId)
    {
        _logger.LogInformation("Пользователь {userId}: Проверка детали {detailId} на наличие тех процессов", HttpContext.User.FindFirstValue("UserId"), detailId);
        
        return Ok(await _service.CheckAvailabilityAsync(detailId));
    }

    /// <summary>
    /// Проверка списка деталей, есть ли у них тех процессы
    /// </summary>
    /// <param name="detailsId">Список Id деталей</param>
    /// <returns>Словарь</returns>
    [HttpGet]
    [Authorize(Roles = TpSupervisorControllerRoles.CheckRangeAvailabilities)]
    public async Task<ActionResult<Dictionary<int, bool>>> CheckRangeAvailabilities([FromQuery, MinLength(1)] List<int> detailsId)
    {
        _logger.LogInformation("Пользователь {userId}: Проверка детали {detailsId} на наличие тех процессов", HttpContext.User.FindFirstValue("UserId"), detailsId);
        
        return Ok(await _service.CheckRangeAvailabilitiesAsync(detailsId));
    }

    /// <summary>
    /// Получение списка всех тех процессов определенной детали
    /// </summary>
    /// <param name="detailId">Id детали</param>
    /// <returns>Список тех процессов и короткая информация об детали</returns>
    [HttpGet]
    [Authorize(Roles = TpSupervisorControllerRoles.GetAllDetailTps)]
    public async Task<ActionResult<GetDetailTechProcessesDto>> GetAllDetailTps([FromQuery, Range(1, int.MaxValue)] int detailId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех тех процессов детали {detailId}", HttpContext.User.FindFirstValue("UserId"), detailId);
       
        return Ok(await _service.GetDetailTpsAsync(detailId));
    }

    /// <summary>
    /// Получение словаря где Key - DeveloperId, Value - список тех процессов, которые в данный момент висят на разработчике
    /// </summary>
    /// <param name="developers">Список Id разработчиков</param>
    /// <param name="productId">Id изделия для дополнительной фильтрации</param>
    /// <returns>Словарь или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = TpSupervisorControllerRoles.GetAllDevelopersTasks)]
    public async Task<ActionResult<List<GetAllDevelopersTasksDto>>> GetAllDevelopersTasks(
        [FromQuery, MinLength(1)] List<int> developers, 
        [FromQuery, DefaultValue(0), Range(0, int.MaxValue)] int productId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение технологов разработчиков с текущими задачами", HttpContext.User.FindFirstValue("UserId"));
        
        var developersWithTasks = await _service.GetAllDevelopersTasksAsync(developers, productId);
        
        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(developersWithTasks);
    }
}