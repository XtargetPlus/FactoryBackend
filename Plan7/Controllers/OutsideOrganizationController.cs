using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;
using ServiceLayer.IServicesRepository.IOutsideOrganizationCountServices;
using Plan7.Helper.Controllers.Roles;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер сторонней организации
/// </summary>
public class OutsideOrganizationController : BaseReactController<OutsideOrganizationController>
{
    private readonly IOutsideOrganizationService _service;
    private readonly IOutsideOrganizationCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public OutsideOrganizationController(
        ILogger<OutsideOrganizationController> logger, 
        IOutsideOrganizationService service, 
        IOutsideOrganizationCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Наименование</param>
    /// <returns>Id или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = OutsideOrganizationControllerRoles.Add)]
    public async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление сторонней организации: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);

        var id = await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация об сторонней информации</param>
    /// <returns>Ok и или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = OutsideOrganizationControllerRoles.Change)]
    public async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение сторонней организации: {outsideOrganizationId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id записи на удаление</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = OutsideOrganizationControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление сторонней организации: {outsideOrganizationId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Список сторонних организаций
    /// </summary>
    /// <param name="take">Сколько получить</param>
    /// <param name="skip">Сколько пропустить</param>
    /// <returns>Список сторонних организаций или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = OutsideOrganizationControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery, DefaultValue(50), Range(0, int.MaxValue)] int take, [FromQuery, DefaultValue(0), Range(0, int.MaxValue)] int skip)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка сторонних организаций", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync();

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(take, skip), Length = count });
    }
}
