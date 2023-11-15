using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;

namespace Plan7.Controllers.Tools;

public class ToolParameterController : BaseReactController<ToolParameterController>
{
    private readonly IToolParameterService _service;

    public ToolParameterController(ILogger<ToolParameterController> logger, IToolParameterService service)
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Создание параметра
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddParameter(AddToolParameterDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление параметра {title}",
            HttpContext.User.FindFirstValue("UserId"),dto.Title);
        var id = await _service.AddParameterAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok(id);
    }

    /// <summary>
    /// Изменение параметра
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeParameters(ChangeToolParameterDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение параметра {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.Id);
        await _service.ChangeParameterAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Удаление параметра
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteParameters(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление параметра {id}",
            HttpContext.User.FindFirstValue("UserId"),id);
        await _service.DeleteParameterAsync(id);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Получение всех параметров
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolParameterDto>?>> GetAllParameters()
    {
        _logger.LogInformation("Пользователь {userId}: Выгрузка всех параметров",
            HttpContext.User.FindFirstValue("UserId"));
        return Ok(await _service.GetParametersAsync());
    }
}
