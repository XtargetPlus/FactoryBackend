using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Admin;
using ServiceLayer.IServicesRepository;
using Shared.Dto;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Plan7.Controllers.Admin;

/// <summary>
/// Контроллер ролей
/// </summary>
public class RoleController : SimpleController<BaseDto, RoleController>
{
    private readonly IRoleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RoleController(ILogger<RoleController> logger, IRoleService service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = RoleControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление роли: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        var id = await _service.AddAsync(dto);

        return id is null or 0 ? BadRequest() : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = RoleControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение роли: {roleId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление роли: {roleId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(id);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();

    }

    /// <summary>
    /// Получение списка ролей
    /// </summary>
    /// <returns>Список ролей</returns>
    [HttpGet]
    [Authorize(Roles = RoleControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка ролей", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync(text));
    }
}
