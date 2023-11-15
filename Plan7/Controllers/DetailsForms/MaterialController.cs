using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
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
/// Контроллер материала
/// </summary>
public class MaterialController : SimpleController<BaseDto, MaterialController>
{
    private readonly ISimpleGenericModelService<Material> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public MaterialController(ILogger<MaterialController> logger, ISimpleGenericModelService<Material> service)
        : base(logger) => _service = service;

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = MaterialControllerRoles.Add)]
    public override async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление материала: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        var id = await _service.AddAsync(new() { Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = MaterialControllerRoles.Change)]
    public override async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение материала: {MaterialId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(new() { Id = dto.Id, Title = dto.Title });
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = MaterialControllerRoles.Delete)]
    public override async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление материала: {MaterialId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await _service.DeleteAsync(new() { Id = id });
        
        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка материалов
    /// </summary>
    /// <returns>Список материалов</returns>
    [HttpGet]
    [Authorize(Roles = MaterialControllerRoles.GetAll)]
    public override async Task<IActionResult> GetAll([FromQuery, MaybeNull] string text)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка материалов", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllAsync(text));
    }
}
