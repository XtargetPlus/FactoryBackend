using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.EquipmentForms;
using ServiceLayer.Equipments.Services.Interfaces;
using Shared.Dto.Detail;
using Shared.Dto.Equipment.Filters;
using System.Security.Claims;

namespace Plan7.Controllers.EquipmentForms;

/// <summary>
/// Контроллер детали для станков
/// </summary>
public class EquipmentDetailController : BaseReactController<EquipmentDetailController>
{
    private readonly IEquipmentDetailService _service;
    private readonly IEquipmentDetailCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public EquipmentDetailController(
        ILogger<EquipmentDetailController> logger, 
        IEquipmentDetailService service,
        IEquipmentDetailCountService count)
        : base(logger) 
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление 
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentDetailControllerRoles.Add)]
    public async Task<IActionResult> Add(BaseSerialTitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление детали станков: {SerialNumber} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.SerialNumber, dto.Title);
        
        var id = await _service.AddAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Редактирование
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentDetailControllerRoles.Change)]
    public async Task<IActionResult> Change(BaseIdSerialTitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение детали станков: {detailId} - {SerialNumber} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.DetailId, dto.SerialNumber, dto.Title);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = EquipmentDetailControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление детали станков: {detailId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка деталей для станков
    /// </summary>
    /// <param name="filters">Фильтры для выборки</param>
    /// <returns>Список деталей для станков</returns>
    [HttpGet]
    [Authorize(Roles = EquipmentDetailControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEquipmentDetailFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка деталей станков", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync(filters.Text, filters.SearchOptions);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение списка деталей станка
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список деталей станка или ошибка, если equipmentId меньше или равно 0</returns>
    [HttpGet]
    [Authorize(Roles = EquipmentDetailControllerRoles.GetAllFromEquipment)]
    public async Task<ActionResult<List<BaseSerialTitleDto>>> GetAllFromEquipment([FromQuery] GetAllEquipmentDetailsFromEquipmentFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка деталей станка {EquipmentId}", HttpContext.User.FindFirstValue("UserId"), filters.EquipmentId);
        
        return Ok(await _service.GetAllFromEquipmentAsync(filters));
    }
}
