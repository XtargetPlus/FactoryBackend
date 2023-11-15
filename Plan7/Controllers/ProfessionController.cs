using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles;
using ServiceLayer.IServicesRepository.IProfessionServices;
using Shared.Dto;
using Shared.Dto.Profession.Filters;
using System.Security.Claims;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер профессий
/// </summary>
public class ProfessionController : BaseReactController<ProfessionController>
{
    private readonly IProfessionService _service;
    private readonly IProfessionCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public ProfessionController(
        ILogger<ProfessionController> logger, 
        IProfessionService service, 
        IProfessionCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление профессии
    /// </summary>
    /// <param name="dto">Наименование профессии</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = ProfessionControllerRoles.Add)]
    public async Task<IActionResult> Add(TitleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление профессии: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);

        var id = await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение профессии
    /// </summary>
    /// <param name="dto">Информация о профессии</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = ProfessionControllerRoles.Change)]
    public async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение профессии: {ProfessionId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);

        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление профессии
    /// </summary>
    /// <param name="id">Id профессии</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = ProfessionControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление профессии: {ProfessionId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка профессий
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    [HttpGet]
    [Authorize(Roles = ProfessionControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProfessionFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка профессий", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync(filters.Text);

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }
}
