using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;
using Shared.Dto.Tools.Tool.Filters;
using Shared.Dto.Tools.ToolChild.Filters;
using Shared.Dto.Tools.ToolEquipment.Filters;
using Shared.Dto.Tools.ToolParameters.Filters;
using Shared.Dto.Tools.ToolReplaceability.Filters;

namespace Plan7.Controllers.Tools;

public class ToolController : BaseReactController<ToolController>
{
    private readonly IToolsService _service;

    public ToolController(ILogger<ToolController> logger, IToolsService service) 
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddTool(AddToolDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление инструмента {title}",
            HttpContext.User.FindFirstValue("UserId"),dto.Title);
        var id = await _service.AddAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok(id);
    }

    /// <summary>
    /// Добавление зависимого инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddChild(AddToolChildrenDto dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление дочернего инструмента {childId} к {fatherId}",
            HttpContext.User.FindFirstValue("UserId"), dto.ChildrenId, dto.FatherId);
        await childService.AddChildAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Добавление параметра к инструменту
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="toolParametersService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddParameters(AddToolParametersListDto dto,
        [FromServices] IToolParameterConcreteService toolParametersService)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление параметров к {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.ToolId);
        await toolParametersService.AddRangeParametersAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Добавление заменяемости к инструменту
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="replaceabilityService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReplaceability(AddReplaceabilityDto dto, [FromServices] IToolReplaceabilityService replaceabilityService)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление заменяемости к {id}",
            HttpContext.User.FindFirstValue("UserId"), dto.MasterId);
        await replaceabilityService.AddReplaceabilityAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Добавлние станка к инструменту
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="equipmentService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddEquipment(AddToolEquipmentDto dto,
        [FromServices] IToolEquipmentService equipmentService)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление станка {equipmentId} к инструменту {toolId}",
            HttpContext.User.FindFirstValue("UserId"),dto.EquipmentId,dto.ToolId);
        await equipmentService.AddEquipmentAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Изменение инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeTool(ChangeToolDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение инструмента {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.Id);
        await _service.ChangeAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Изменение дочернего инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeChild(ChangeToolChildDto dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение дочернего инструмента {childId}",
            HttpContext.User.FindFirstValue("UserId"), dto.ChildId);
        await childService.ChangeChildAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Изменение порядка инструмента
    /// </summary>
    /// <param name="dto">Параметры</param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SwapChild(SwapToolChildDto dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение приоритета инструмента {old}->{new}",
            HttpContext.User.FindFirstValue("UserId"), dto.NewPrioryty, dto.OldPrioryty);
        await childService.SwapChildAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Вставка между дочерними инструментами
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> InsertBetweenChild(SwapToolChildDto dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("");
        await childService.InsertBetweenAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Изменение параметра
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="toolParameterService"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeParameters(AddToolParametersListDto dto, [FromServices] IToolParameterConcreteService toolParameterService)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение параметров {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.ToolId);
        await toolParameterService.ChangeRangeParametersAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    // TODO: Это не нужно
    //[HttpPost]
    //[Authorize]
    //public async Task<IActionResult> ChangeReplaceability(AddReplaceabilityDto dto,
    //    [FromServices] IToolReplaceabilityService replaceabilityService)
    //{
    //    _logger.LogInformation("Пользователь {userId}: Изменение заменяемости у инструмента {id}",
    //        HttpContext.User.FindFirstValue("UserId"), dto.MasterId);
    //    await replaceabilityService.ChangeReplaceabilityAsync(dto);
    //    return _service.HasWarningsOrErrors
    //        ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
    //        : Ok();
    //}

    /// <summary>
    /// Удаление инструмента
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление инструмента {id}",
            HttpContext.User.FindFirstValue("UserId"), id);
        await _service.DeleteAsync(id);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Удаление дочернего инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteChild([FromQuery] DeleteToolChildDto dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление дочернего инструмента {child}",
            HttpContext.User.FindFirstValue("UserId"), dto.ChildId);
        await childService.DeleteChildAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Удаление заменяемости
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="replaceabilityService"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteReplaceability([FromQuery] AddReplaceabilityDto dto,
        [FromServices] IToolReplaceabilityService replaceabilityService)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление заменяемости {child}",
            HttpContext.User.FindFirstValue("UserId"), dto.SlaveId);
        await replaceabilityService.DeleteReplaceabilityAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Удаление параметра
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="parameterService"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteParameters([FromQuery] DeleteToolParametersDto dto, [FromServices] IToolParameterConcreteService parameterService)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление параметров у инструмента {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.ToolId);
        await parameterService.DeleteRangeParametersAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Удаление станка
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="equipmentService"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteEquipment([FromQuery] AddToolEquipmentDto dto,
        [FromServices] IToolEquipmentService equipmentService)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление станка {id}", HttpContext.User.FindFirstValue("UserId"), dto.EquipmentId);
        await equipmentService.DeleteEquipmentAsync(dto);
        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok();
    }

    /// <summary>
    /// Получение инструментов по каталогу
    /// </summary>
    /// <param name="catalogId"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolDto>?>> GetAll([FromQuery, Range(1, int.MaxValue)] int catalogId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение инструментов каталога {id}",
            HttpContext.User.FindFirstValue("UserId"),catalogId);
        
        return Ok(await _service.GetForCatalogAsync(catalogId));
    }


    /// <summary>
    /// Получение инструмента по параметрам
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<List<GetToolDto>?>> GetAllWithParameters(GetToolWithParametersDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение инструмента по параметрам");
        return Ok(await _service.GetWithParameters(dto));
    }

    /// <summary>
    /// Получение дочерних инструментов
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolCatalogDto>?>> GetAllChild([FromQuery]GetAllChildrenFilters dto, [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Выгрузка всех дочерних инструментов инструмента {father}",
            HttpContext.User.FindFirstValue("UserId"),dto.FatherId);
        return Ok(await childService.GetAllAsync(dto));
    }

    /// <summary>
    /// Получение родительских инструментов
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="childService"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolCatalogDto>?>> GetAllFather([FromQuery] GetAllFatherFilters dto,
        [FromServices] IToolChildService childService)
    {
        _logger.LogInformation("Пользователь {userId}: Выгрузка всех родительских инструментов инструмента {child}",
            HttpContext.User.FindFirstValue("UserId"), dto.ChildId);
        return Ok(await childService.GetAllFatherAsync(dto));
    }

    /// <summary>
    /// Получение списка параметров инструмента
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="parameterService"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolParametersDto>?>> GetAllParameters([FromQuery] GetAllParametersFilters dto, [FromServices] IToolParameterConcreteService parameterService)
    {
        _logger.LogInformation("Пользователь {userId}: Получение параметров инструмента {id}",
            HttpContext.User.FindFirstValue("UserId"),dto.ToolId);
        return Ok(await parameterService.GetAllRangeParametersAsync(dto));
    }

    /// <summary>
    /// Получение списка заменяемости инструмента
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="replaceabilityService"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolReplaceabilityDto>?>> GetAllReplaceability(
        [FromQuery] GetAllReplaceabilityFilters filters, [FromServices] IToolReplaceabilityService replaceabilityService)
    {
        _logger.LogInformation("Пользователь {userId}: Получение заменяемости инструмента {id}", HttpContext.User.FindFirstValue("UserId"), filters.ToolId);
        return Ok(await replaceabilityService.GetReplaceabilityAsync(filters));
    }

    /// <summary>
    /// Получение списка станков для инструмента
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="equipmentService"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetToolEquipmentDto>?>> GetAllEquipment([FromQuery] GetAllEquipmentFilters filters,
        [FromServices] IToolEquipmentService equipmentService)
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех станков для инструмента {id}", HttpContext.User.FindFirstValue("UserId"), filters.Id);
        return Ok(await equipmentService.GetEquipmentAsync(filters));
    }
}