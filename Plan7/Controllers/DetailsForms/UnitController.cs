using DB.Model.DetailInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.DetailsForms;
using ServiceLayer.IServicesRepository;
using Shared.Dto;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Plan7.Controllers.DetailsForms;

/// <summary>
/// Контроллер единицы измерения
/// </summary>
public class UnitController : SimpleController<BaseDto, UnitController>
{
    private readonly ISimpleGenericModelService<Unit> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public UnitController(ILogger<UnitController> logger, ISimpleGenericModelService<Unit> service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = UnitControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление единицы измерения: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        var id = await _service.AddAsync(new() { Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = UnitControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение единицы измерения: {UnitId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UnitControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление единицы измерения: {UnitId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(new() { Id = id });

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка единиц измерения
    /// </summary>
    /// <returns>Количество единиц измерения</returns>
    [HttpGet]
    [Authorize(Roles = UnitControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка единиц измерений", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetAllAsync(text));
    }
}
