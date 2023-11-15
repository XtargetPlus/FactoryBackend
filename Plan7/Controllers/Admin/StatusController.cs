using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Admin;
using ServiceLayer.IServicesRepository;
using Shared.Dto.Status;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Plan7.Controllers.Admin;

/// <summary>
/// Контроллер статусов
/// </summary>
public class StatusController : BaseReactController<StatusController>
{
    private readonly IStatusService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public StatusController(ILogger<StatusController> logger, IStatusService service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = StatusControllerRoles.Add)]
    public async Task<IActionResult> Add(StatusChangeDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление статуса: {Title} - {TableName}", HttpContext.User.FindFirstValue("UserId"), dto.Title, dto.TableName);
        
        var id = await _service.AddAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = StatusControllerRoles.Change)]
    public async Task<IActionResult> Change(StatusDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение статуса: {statusId} - {Title} - {TableName}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title, dto.TableName);
        
        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = StatusControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление статуса: {statusId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(id);
        
        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получение списка статусов
    /// </summary>
    /// <returns>Список статусов</returns>
    [HttpGet]
    [Authorize(Roles = StatusControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка статусов", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync(0, 0));
    }

    /// <summary>
    /// Получение списка статусов для определенной таблицы
    /// </summary>
    /// <param name="tableName">Наименование таблицы</param>
    /// <returns>Список статусов</returns>
    [HttpGet]
    [Authorize(Roles = StatusControllerRoles.GetTableStatuses)]
    public async Task<IActionResult> GetTableStatuses([FromQuery, MaybeNull] string tableName)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка статусов по таблице {tableName}", HttpContext.User.FindFirstValue("UserId"), tableName);
        
        return Ok(await _service.GetTableStatusesAsync(tableName));
    }
}
