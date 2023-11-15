using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.TechnologicalProcesses;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using Shared.Dto.TechnologicalProcess;
using System.Security.Claims;

namespace Plan7.Controllers.TechnologicalProcesses;

/// <summary>
/// Контроллер архива тех процессов
/// </summary>
public class TpArchiveController : BaseReactController<TpArchiveController>
{
    private readonly ITpArchiveService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TpArchiveController(ILogger<TpArchiveController> logger, ITpArchiveService service)
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Выдать тех процесс
    /// </summary>
    /// <param name="dto">Информация для выдачи, где
    /// TechProcessId - id тех процесса; Note - новое примечание, возможен null; TechProcessStatusId - id статуса, который может устанавливать склад</param>
    /// <returns>Возвращается подробная информация выданного тех процесса для таблицы выданных тех процессов, без учета активных фильтров или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpArchiveControllerRoles.Issue)]
    public async Task<ActionResult<GetAllIssuedTechProcessesDto>> Issue(ArchiveChangeTechProcessStatusDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена статуса тех процесса {TechProcessId} на {StatusId}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, (int)dto.TechProcessStatusId);
        
        await _service.IssueAsync(dto, int.Parse(HttpContext.User.FindFirstValue("UserId")!));
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Выдать дубликат тех процесса
    /// </summary>
    /// <param name="dto">Информация для выдачи дубликата, где
    /// TechProcessId - id тех процесса, чей дубликат нужно выдать; SubdivisionId - в какое подразделение; UserId - какому сотруднику</param>
    /// <returns>Подробная информация о выданном дубликате или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpArchiveControllerRoles.IssueDuplicate)]
    public async Task<ActionResult<GetTechProcessDuplicateDto>> IssueDuplicate(IssueTechProcessDuplicateDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Выдача дубликата тех процесса {techProcessId} пользователю {UserId} из подразделения {SubdivisionId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.UserId, dto.SubdivisionId);
        
        await _service.IssueDuplicateAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление дубликата тех процесса, который был выдан определенному человеку
    /// </summary>
    /// <param name="techProcessStatusId">Id статуса тех процесса для удаления</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpDelete]
    [Authorize(Roles = TpArchiveControllerRoles.DeleteIssuedDuplicate)]
    public async Task<IActionResult> DeleteIssuedDuplicate([FromQuery] int techProcessStatusId)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление дубликата тех процесса {techProcessStatusId}",
            HttpContext.User.FindFirstValue("UserId"), techProcessStatusId);

        await _service.DeleteIssuedDuplicateAsync(techProcessStatusId);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }
}
