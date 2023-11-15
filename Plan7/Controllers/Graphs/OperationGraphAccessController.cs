using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Graphs;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.Access;
using Shared.Dto.Users;

namespace Plan7.Controllers.Graphs;

/// <summary>
/// 
/// </summary>
public class OperationGraphAccessController : BaseReactController<OperationGraphAccessController>
{
    private readonly IOperationGraphAccessService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OperationGraphAccessController(ILogger<OperationGraphAccessController> logger, IOperationGraphAccessService service) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Предоставляем доступ к графику сторонним пользователям с уровнями доступа
    /// </summary>
    /// <param name="dto">Информация для предоставления доступа, где в словаре Key - userId, Value - isReadonly</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphAccessControllerRoles.GiveAccess)]
    public async Task<IActionResult> GiveAccess(GiveAccessGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Предоставление доступа пользователям {users}, к графику {graphId}",
            HttpContext.User.FindFirstValue("UserId"), string.Join(", ", dto.UserAccesses), dto.OperationGraphId);

        await _service.GiveAccessAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена владельца графика с возможностью оставления у старого владельца прав доступа на чтение или редактирование
    /// </summary>
    /// <param name="dto">Информация для смены владельца.
    /// Где NewUserAccess: 0 - None, 1 - ReadAndEdit, 2 - Readonly</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphAccessControllerRoles.ChangeOwner)]
    public async Task<IActionResult> ChangeOwner(ChangeGraphOwnerDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена вледальца графика {graphId} на пользователя {newOwnerId}, права для старого владельца {newAccess}",
            HttpContext.User.FindFirstValue("UserId"), dto.OperationGraphId, dto.NewOwnerId, dto.NewUserAccess);

        await _service.ChangeOwnerAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Меняем пользователю права доступа к графику
    /// </summary>
    /// <param name="dto">Информация для смены прав доступа. UserAccess: 1 - Readonly, 2 - Full</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphAccessControllerRoles.ChangeUserAccess)]
    public async Task<IActionResult> ChangeUserAccess(ChangeUserAccessDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена пользователю {graphUserId} уровня доступа к графику {graphId} на {newAccess}",
            HttpContext.User.FindFirstValue("UserId"), dto.UserId, dto.GraphId, dto.NewAccess);

        await _service.ChangeUserAccess(dto);

        return _service.HasWarningsOrErrors? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Отбираем у пользователя доступ к графику
    /// </summary>
    /// <param name="dto">Информация для изъятия прав</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpDelete]
    [Authorize(Roles = OperationGraphAccessControllerRoles.RevokeAccess)]
    public async Task<IActionResult> RevokeAccess(RevokeGraphAccessDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Лишение пользователя {graphUserId} доступа к графику {graphId}",
            HttpContext.User.FindFirstValue("UserId"), string.Join(", ", dto.UsersId), dto.GraphId);

        await _service.RevokeAccess(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получение списка пользователей, имеющих доступ к переденному графику
    /// </summary>
    /// <param name="graphId">Id операционного графика</param>
    /// <returns>Список пользователей, которые имеют доступ к графику</returns>
    [HttpGet("{graphId:int}")]
    [Authorize(Roles = OperationGraphAccessControllerRoles.AllWithAccessToOperationGraph)]
    public async Task<ActionResult<List<GetAllUserGraphAccessDto>>> AllWithAccessToOperationGraph([FromRoute] int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка пользователей с доступом к графику {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        return Ok(await _service.AllWithAccessToOperationGraphAsync(graphId));
    }

    /// <summary>
    /// Получение списка сотрудников, которые не имеют доступа к графику
    /// </summary>
    /// <param name="graphId">Id операционного графика</param>
    /// <returns>Список пользователей без доступа к графику</returns>
    [HttpGet("{graphId:int}")]
    [Authorize(Roles = OperationGraphAccessControllerRoles.AllWithoutAccessToOperationGraph)]
    public async Task<ActionResult<List<UserGetWithSubdivisionDto>>> AllWithoutAccessToOperationGraph([FromRoute] int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка пользователей без доступа к графику {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        return Ok(await _service.AllWithoutAccessToOperationGraphAsync(graphId));
    }

    /// <summary>
    /// Получение списка типов доступа к графику
    /// </summary>
    /// <returns>Список типов доступа к графику</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphAccessControllerRoles.AccessTypes)]
    public ActionResult<Dictionary<int, string>> AccessTypes()
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех возможных прав доступа к графику", HttpContext.User.FindFirstValue("UserId"));

        return Ok(_service.AccessTypes());
    }
}