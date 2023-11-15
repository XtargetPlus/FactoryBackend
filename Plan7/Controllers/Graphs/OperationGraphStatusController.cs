using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Graphs;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.Status;
using System.Security.Claims;

namespace Plan7.Controllers.Graphs;

public class OperationGraphStatusController : BaseReactController<OperationGraphStatusController>
{
    private readonly IOperationGraphStatusService _service;

    public OperationGraphStatusController(ILogger<OperationGraphStatusController> logger, IOperationGraphStatusService service) : base(logger)
    {
        _service = service;
    }
    
    /// <summary>
    /// Смена статуса операционного графика
    /// </summary>
    /// <param name="dto">Информация для смены статуса</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphStatusControllerRoles.Change)]   
    public async Task<IActionResult> Change(OperationGraphChangeStatusDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена статуса для графика {graphId}, новый статус - {statusId}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphId, dto.StatusId);

        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }
}