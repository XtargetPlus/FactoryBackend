using DB.Model.StorageInfo;
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
/// Контроллер типа движения
/// </summary>
public class MoveTypeController : SimpleController<BaseDto, MoveTypeController>
{
    private readonly ISimpleGenericModelService<MoveType> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public MoveTypeController(ILogger<MoveTypeController> logger, ISimpleGenericModelService<MoveType> service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = MoveTypeControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление типа движения: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);

        var id = await _service.AddAsync(new() { Title = dto.Title });

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация о типе деталей</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = MoveTypeControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение типа движения: {moveTypeId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);

        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id записи на удаление</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = MoveTypeControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление типа движения: {moveTypeId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(new() { Id = id });

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка типов деталей
    /// </summary>
    /// <returns>Список типов деталей</returns>
    [HttpGet]
    [Authorize(Roles = MoveTypeControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка типов движений", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetAllAsync(text));
    }
}
