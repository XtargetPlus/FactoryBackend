using DB.Model.AccessoryInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles;
using ServiceLayer.IServicesRepository;
using Shared.Dto;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер типа оснастки
/// </summary>
public class AccessoryTypeController : SimpleController<BaseDto, AccessoryTypeController>
{
    private readonly ISimpleGenericModelService<AccessoryType> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public AccessoryTypeController(ILogger<AccessoryTypeController> logger, ISimpleGenericModelService<AccessoryType> service) : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = AccessoryTypeControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление типа оснастки: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);

        var id = await _service.AddAsync(new() { Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация о типе оснастки</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = AccessoryTypeControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение типа оснастки: {AccessoryTypeId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
       
        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = AccessoryTypeControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление типа оснастки: {AccessoryTypeId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(new() { Id = id });

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка типов оснасток
    /// </summary>
    /// <returns>Список оснасток</returns>
    [HttpGet]
    [Authorize(Roles = AccessoryTypeControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка типов оснастки", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetAllAsync(text));
    }
}
