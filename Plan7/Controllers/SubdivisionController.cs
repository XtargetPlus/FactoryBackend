using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles;
using ServiceLayer.IServicesRepository;
using Shared.Dto;
using Shared.Dto.Subdiv;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер подразделения
/// </summary>
public class SubdivisionController : BaseReactController<SubdivisionController>
{
    private readonly ISubdivisionService _service; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public SubdivisionController(ILogger<SubdivisionController> logger, ISubdivisionService service)
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление подразделения
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = SubdivisionControllerRoles.Add)]
    public async Task<IActionResult> Add(BaseSubdivisionDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление подразделения {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);
        
        var id = await _service.AddValueAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение подразделения
    /// </summary>
    /// <param name="dto">Информация о подразделении</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = SubdivisionControllerRoles.Change)]
    public async Task<IActionResult> Change(BaseDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение подразделения: {subdivisionId} - {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title);
        
        await _service.ChangeAsync(dto);
        
        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление подразделения
    /// </summary>
    /// <param name="id">Id подразделения</param>
    /// <returns>Ok или ошибка при удалении</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = SubdivisionControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление подразделения: {subdivisionId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Список подразделений определенного уровня
    /// </summary>
    /// <param name="fatherId">Id родительского подразделения или null</param>
    /// <returns>Список подразделений определенного уровня или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = SubdivisionControllerRoles.GetAllLevel)]
    public async Task<ActionResult<List<SubdivisionGetDto>>> GetLevel([FromQuery, DefaultValue(null)] int? fatherId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение уровня подразделений {fatherId}", HttpContext.User.FindFirstValue("UserId"), fatherId);

        var subdivs = await _service.GetAllLevelAsync(fatherId);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(subdivs);
    }

    /// <summary>
    /// Список подразделений с учетом всех уровней (п1: п2: п3: п4)
    /// </summary>
    /// <returns>Полный список подразделений или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = SubdivisionControllerRoles.GetAll)]
    public async Task<ActionResult<List<BaseDto>>> GetAll()
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка подразделений", HttpContext.User.FindFirstValue("UserId"));

        var subdivs = await _service.GetAllAsync();

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(subdivs);
    }

    /// <summary>
    /// Список подразделений с учетом всех уровней (п1: п2: п3: п4) и наличию станков
    /// </summary>
    /// <returns>Полный список подразделений или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = SubdivisionControllerRoles.GetAllByEquipmentContain)]
    public async Task<ActionResult<List<BaseDto>>> GetAllByEquipmentContain([FromQuery, DefaultValue(false)] bool isContainEquipments)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка подразделений с учетом наличия станков", HttpContext.User.FindFirstValue("UserId"));

        var subdivs = await _service.GetAllByEquipmentContainAsync(isContainEquipments);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(subdivs);
    }

    /// <summary>
    /// Получаем список подразделений без тех процесса
    /// </summary>
    /// <param name="techProcessId">Id тех процесса</param>
    /// <returns>Список подразделений</returns>
    [HttpGet]
    [Authorize(Roles = SubdivisionControllerRoles.GetAllWithoutTechProcess)]
    public async Task<ActionResult<List<BaseDto>>> GetAllWithoutTechProcess([FromQuery, Range(1, int.MaxValue)] int techProcessId)
    {
        _logger.LogInformation("Пользователь {UserId}: Получение списка подразделений без тех процесса {techProcessId}", HttpContext.User.FindFirstValue("UserId"), techProcessId);
        
        var subdivs = await _service.GetAllWithoutTechProcessAsync(techProcessId);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok(subdivs);
    }
}
