using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Admin;
using ServiceLayer.IServicesRepository;
using Shared.Dto.Role;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Plan7.Controllers.Admin;

/// <summary>
/// Функционалы ролей на формах
/// </summary>
public class RoleClientController : BaseReactController<RoleClientController>
{
    private readonly IRoleClientService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RoleClientController(ILogger<RoleClientController> logger, IRoleClientService service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = RoleClientControllerRoles.Add)]
    public async Task<IActionResult> Add(RoleClientDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление функционала к роли {roleId} и форме {clientFormId}:  {func}", 
            HttpContext.User.FindFirstValue("UserId"), dto.RoleId, dto.UserFormId, dto.Func);
        
        await _service.AddValueAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Множественное добавление функционала на формах к роли 
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddRange(RoleAddRangeWithFuncOnFormsDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление множества функционалов к роли {roleId}", HttpContext.User.FindFirstValue("UserId"), dto.RoleId);
        
        await _service.AddRangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = RoleClientControllerRoles.Change)]
    public async Task<IActionResult> Change(RoleClientDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение функционала роли {roleId} на форме {clientFormId}: {func}", HttpContext.User.FindFirstValue("UserId"), dto.RoleId, dto.UserFormId, dto.Func);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Множественное изменение функционала на формах для роли
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = RoleClientControllerRoles.ChangeRange)]
    public async Task<IActionResult> ChangeRange(RoleChangeRangeWithFuncOnFormsDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение множества функционалов к роли {roleId}", HttpContext.User.FindFirstValue("UserId"), dto.RoleId);
        
        await _service.ChangeRangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpDelete]
    [Authorize(Roles = RoleClientControllerRoles.Delete)]
    public async Task<IActionResult> Delete([FromQuery] BaseRoleClientDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление функционала роли {roleId} на форме {clientFormId}", HttpContext.User.FindFirstValue("UserId"), dto.RoleId, dto.UserFormId);
        
        await _service.DeleteAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Множественное удаления функционала на формах для роли
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpDelete]
    [Authorize(Roles = RoleClientControllerRoles.DeleteRange)]
    public async Task<IActionResult> DeleteRange([FromQuery] RoleClientDeleteRangeDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение множества функционалов к роли {roleId}", HttpContext.User.FindFirstValue("UserId"), dto.RoleId);
        
        await _service.DeleteRangeAsync(dto);
        
        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получаем список всех форм и ролей с функционалом
    /// </summary>
    /// <returns>Список всего вот этого</returns>
    [HttpGet]
    [Authorize(Roles = RoleClientControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка форм и ролей с функционалом", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// Получаем список форм, которые может видеть пользователь
    /// </summary>
    /// <param name="forms">Массив форм</param>
    /// <returns>Результирующий список только тех форм, которые видит пользователь</returns>
    [HttpGet]
    [Authorize(Roles = RoleClientControllerRoles.GetUserForms)]
    public async Task<IActionResult> GetUserForms([FromQuery, MinLength(1)] List<string> forms)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка форм пользователя {forms}", HttpContext.User.FindFirstValue("UserId"), forms);
        
        return Ok(await _service.GetUserFormsAsync(forms, HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) ?? ""));
    }

    /// <summary>
    /// Получаем функционал пользователя на этой форме
    /// </summary>
    /// <param name="form">Наименование формы</param>
    /// <returns>Функционал пользователя на этой форме</returns>
    [HttpGet]
    [Authorize(Roles = RoleClientControllerRoles.GetFormFunc)]
    public async Task<IActionResult> GetFormFunc([FromQuery, MinLength(1)] string form)
    {
        _logger.LogInformation("Пользователь {userId}: Получение функционала формы {form}", HttpContext.User.FindFirstValue("UserId"), form);
        
        return Ok(await _service.GetFormFuncAsync(form, HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) ?? ""));
    }
}
