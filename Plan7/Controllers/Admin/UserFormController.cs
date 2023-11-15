using DB.Model.UserInfo.RoleInfo;
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
/// Контроллер пользовательских форм
/// </summary>
public class UserFormController : SimpleController<BaseDto, UserFormController>
{
    private readonly ISimpleGenericModelService<UserForm> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public UserFormController(ILogger<UserFormController> logger, ISimpleGenericModelService<UserForm> service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование формы</param>
    /// <returns>Id добавленной записи или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = UserFormControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление формы: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        var id = await _service.AddAsync(new() { Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpPost]
    [Authorize(Roles = UserFormControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение формы: {formId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto);
        
        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или BadRequest</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserFormControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление формы: {formId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(new() { Id = id });

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получаем список форм
    /// </summary>
    /// <returns>Список форм</returns>
    [HttpGet]
    [Authorize(Roles = UserFormControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull, DefaultValue("")] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка форм", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync(text));
    }
}
