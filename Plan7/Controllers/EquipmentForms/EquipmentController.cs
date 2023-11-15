using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Plan7.Helper.Controllers.Roles.EquipmentForms;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;
using ServiceLayer.Equipments.Services.Interfaces;

namespace Plan7.Controllers.EquipmentForms;

/// <summary>
/// Контроллер станков
/// </summary>
public class EquipmentController : BaseReactController<EquipmentController>
{
    private readonly IEquipmentService _service;
    private readonly IEquipmentCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public EquipmentController(ILogger<EquipmentController> logger, IEquipmentService service, IEquipmentCountService count)
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
    [Authorize(Roles = EquipmentControllerRoles.Add)]
    public async Task<IActionResult> Add(AddEquipmentDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление станка: {Title} - {SerialNumber}", HttpContext.User.FindFirstValue("UserId"), dto.Title, dto.SerialNumber);
        
        var id = await _service.AddAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Добавление детали к станку
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentControllerRoles.AddDetail)]
    public async Task<IActionResult> AddDetail(EquipmentWithDetailDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление детали {equipmentDetailId} к станку {equipmentId}", HttpContext.User.FindFirstValue("UserId"), dto.EquipmentDetailId, dto.EquipmentId);
        
        await _service.AddDetailAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = EquipmentControllerRoles.Change)]
    public async Task<IActionResult> Change(ChangeEquipmentDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение станка {equipmentId}: {Title} - {SerialNumber}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title, dto.SerialNumber);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = EquipmentControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление станка: {equipmentId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Удаление детали из станка
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete]
    [Authorize(Roles = EquipmentControllerRoles.DeleteDetail)]
    public async Task<IActionResult> DeleteDetail([FromQuery] EquipmentWithDetailDto dto)
    {
        _logger.LogInformation("Пользователь {UserId}: Удаление детали {EquipmentDetailId} из станка {EquipmentId}", HttpContext.User.FindFirstValue("UserId"), dto.EquipmentDetailId, dto.EquipmentId);
        
        await _service.DeleteDetailAsync(dto);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Список станков
    /// </summary>
    /// <param name="filters">фильтры выборки</param>
    /// <returns>Список станков с общим количеством или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = EquipmentControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEquipmentFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка станков", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await _count.GetAllAsync(filters.SubdivisionId, filters.Text, filters.SearchOptions);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение списка станков по подразделению
    /// </summary>
    /// <param name="subdivisionId">Id подразделения</param>
    /// <returns>Список станков</returns>
    [HttpGet("{subdivisionId:int}")]
    [Authorize(Roles = EquipmentControllerRoles.GetAllBySubdivision)]
    public async Task<IActionResult> GetAllBySubdivision(int subdivisionId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка станков подразделения {subdivisionId}",
            HttpContext.User.FindFirstValue("UserId"), subdivisionId);

        return Ok(await _service.GetAllBySubdivisionAsync(subdivisionId));
    }
    /// <summary>
    /// Получение инструмента к станку
    /// </summary>
    /// <param name="equipmentId">Идентификатор станка</param>
    /// <returns></returns>
    [HttpGet("{equipmentId:int}")]
    [Authorize]
    public async Task<IActionResult> GetAllTools(int equipmentId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка инструментов к станку {equipmentId}",
            HttpContext.User.FindFirstValue("UserId"), equipmentId);

        // TODO: Переписать, по нормальному, используя mapp
        // await _service.GetToolFromEquipmentAsync(equipmentId)

        return Ok();
    }
    
    // TODO: Вынести в отдельный контроллер TpEquipmentOperation
    //[HttpPost]
    //[Authorize]
    //public async Task<IActionResult> AddToolFromEquipmentOperation(AddEquipmentOperationTool dto, [FromServices] IEquipmentOperationToolService equipmentOperationToolService)
    //{
    //    _logger.LogInformation("Пользователь {userId}: Добавление инстумента к операции на станке {equipmentId}",
    //        HttpContext.User.FindFirstValue("UserId"), dto.EquipmentOperationId);
    //    await equipmentOperationToolService.AddEquipmentOperationTool(dto);
    //    return Ok();
    //}
    //[HttpGet]
    //[Authorize]
    //public async Task<ActionResult<GetAllEquipmentOperationToolsDto>> GetAllToolFromEquipmentOperation([FromQuery] GetAllEquipmentOperationToolsFilters dto, [FromServices] IEquipmentOperationToolService equipmentOperationToolService)
    //{
    //    _logger.LogInformation("Пользователь {userId}: Получение списка инструмента к операции на станке {equipmentId}",
    //        HttpContext.User.FindFirstValue("UserId"), dto.EquipmentOperationId);
    //    return Ok(equipmentOperationToolService.GetEquipmentOperationTool(dto));
    //}
}
