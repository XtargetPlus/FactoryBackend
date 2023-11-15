using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.EquipmentForms;
using ServiceLayer.IServicesRepository;
using Shared.Dto;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Plan7.Controllers.EquipmentForms;

/// <summary>
/// Контроллер причин поломки
/// </summary>
public class EquipmentFailureController : SimpleController<BaseDto, EquipmentFailureController>
{
    private readonly ISimpleGenericModelService<EquipmentFailure> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public EquipmentFailureController(ILogger<EquipmentFailureController> logger, ISimpleGenericModelService<EquipmentFailure> service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentFailureControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление причины поломки станка: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        int? id = await _service.AddAsync(new() { Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Редактирование
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentFailureControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение причины поломки станка: {equipmentFailureId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = EquipmentFailureControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление причины поломки станка: {equipmentFailureId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(new() { Id = id });

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка причин поломок
    /// </summary>
    /// <returns>Список причин поломок</returns>
    [HttpGet]
    [Authorize(Roles = EquipmentFailureControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка причин поломки станка", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync(text));
    }
}
