using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ServiceLayer.IServicesRepository.IOperationServices;
using Plan7.Helper.Controllers.Roles;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto.Operation;
using Shared.Dto.Operation.Filters;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер операций
/// </summary>
public class OperationController : BaseReactController<OperationController>
{
    private readonly IOperationService _service;
    private readonly IOperationCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public OperationController(ILogger<OperationController> logger, IOperationService service, IOperationCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Информация об операции</param>
    /// <returns>Id или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = OperationControllerRoles.Add)]
    public async Task<IActionResult> Add(BaseOperationDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление операции: {fullName} - {shortName}", HttpContext.User.FindFirstValue("UserId"), dto.FullName, dto.ShortName);
        
        var id = await _service.AddAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация об операции</param>
    /// <returns>Ok() или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = OperationControllerRoles.Change)]
    public async Task<IActionResult> Change(OperationDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение операции {OperationId}: {fullName} - {shortName}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.FullName, dto.ShortName);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id записи на удаление</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = OperationControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление операции: {OperationId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка операций
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список операций или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = OperationControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOperationFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка операций", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync(filters.Text, filters.SearchOptions);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение списка операций для селектов
    /// </summary>
    /// <returns>Список операций</returns>
    [HttpGet]
    [Authorize(Roles = OperationControllerRoles.GetAllForChoice)]
    public async Task<ActionResult<List<OperationDto>>> GetAllForChoice()
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка операций для селектов", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetAllForChoiceAsync());
    }
}
