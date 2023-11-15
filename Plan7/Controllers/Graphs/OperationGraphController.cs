using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.CUD;
using System.Security.Claims;
using Plan7.Helper.Controllers.Roles.Graphs;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;

namespace Plan7.Controllers.Graphs;

/// <summary>
/// 
/// </summary>
public class OperationGraphController : BaseReactController<OperationGraphController>
{
    private readonly IOperationGraphService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OperationGraphController(ILogger<OperationGraphController> logger, IOperationGraphService service) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление графика
    /// </summary>
    /// <param name="dto">Данные для добавления графика</param>
    /// <returns>id добавленной записи</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphControllerRoles.Add)]
    public async Task<IActionResult> Add(AddGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Создание графика на {GraphDate}, подразделение {SubdivisionId}, изделие {DetailId}, план {PlanCount}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphDate, dto.SubdivisionId, dto.DetailId, dto.PlanCount);

        var result = await _service.AddAsync(dto, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }

    /// <summary>
    /// Редактирование операционного графика
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphControllerRoles.Change)]
    public async Task<IActionResult> Change(ChangeGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Редактирование графика на {graphId}, данные для редактирования:\n" +
                               "Подразделение {subdivisionId}, дата графика {graphDate}, план {count}",
            HttpContext.User.FindFirstValue("UserId"), dto.OperationGraphId, dto.SubdivisionId, dto.GraphDate, dto.PlanCount);

        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Заморозка (пауза) разработки операционного графика 
    /// </summary>
    /// <param name="graphId">Id графика, который нужно заморозить</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Freeze)]
    public async Task<IActionResult> Freeze(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Заморозка графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.FreezeOrUnAsync(graphId, true);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Разморозить (вернуть в работу) разработку операционного графика
    /// </summary>
    /// <param name="graphId">Id замороженного графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Unfreeze)]
    public async Task<IActionResult> Unfreeze(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Разморозка графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.FreezeOrUnAsync(graphId, false);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Перегенерация графика. Сброс всех изменений и возврат к состаянию после создания
    /// </summary>
    /// <param name="graphId">Id графика, который нужно перегенерировать</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Regeneration)]
    public async Task<IActionResult> Regeneration(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Перегенерация графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.RegenerationAsync(graphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Пересчет приоритетов для всех графиков с приоритетом > 0. Было [1, 2, 4, 17], стало [1, 2, 3, 4]
    /// </summary>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphControllerRoles.RecalculateAllGraphsPriorities)]
    public async Task<IActionResult> RecalculateAllGraphsPriorities()
    {
        _logger.LogInformation("Пользователь {userId}: Пересчет приоритетов для всех графиков", HttpContext.User.FindFirstValue("UserId"));

        await _service.RecalculateAllGraphsPrioritiesAsync();

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Подтверждение операционного графика, вместе со всеми его деталями
    /// </summary>
    /// <param name="graphId">Id графика для подтверждения</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Confirm)]
    public async Task<IActionResult> Confirm(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Подтверждение графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.ConfirmAsync(graphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Снятие подтверждения с операционного графика
    /// </summary>
    /// <param name="graphId">Id графика для подтверждения</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Unconfirm)]
    public async Task<IActionResult> Unconfirm(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Снятие подтверждения с графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.UnconfirmAsync(graphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление операционного графика
    /// </summary>
    /// <param name="graphId">Id графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpDelete("{graphId:int}")]
    [Authorize(Roles = OperationGraphControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int graphId)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), graphId);

        await _service.DeleteAsync(graphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Открытие операционного графика
    /// </summary>
    /// <param name="filters">Фильтры открытия</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphControllerRoles.Open)]
    public async Task<ActionResult<OpenOperationGraphDto>> Open([FromQuery] OpenOperationGraphFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: открытие графика {graphId}",
            HttpContext.User.FindFirstValue("UserId"), filters.GraphId);

        var result = await _service.OpenAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }

    /// <summary>
    /// Получение списка операционных графиков с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список операционных графиков</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphControllerRoles.All)]
    public async Task<ActionResult<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>>?>> All([FromQuery] GetAllOperationGraphFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка графиков с доступом", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.AllAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
    }

    /// <summary>
    /// Получение списка графиков с полным доступом, которые не находятся в группе
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список одиночных графиков</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphControllerRoles.AllSinglesForGroup)]
    public async Task<ActionResult<List<GetAllSinglesOperationGraphDto>>> AllSinglesForGroup([FromQuery] GetAllSinglesForGroupFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка одиночных графиков для создания графиков или добавления к существующим графикам",
            HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.AllSinglesForGroupAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
    }

    /// <summary>
    /// Получение списка собственных операционных графиков с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки, наличие изделия и какие графики нужно иганировать</param>
    /// <returns>Возврат списка собственных операционных графиков, которые находятся в разработке или на паузе</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphControllerRoles.AllFromOwner)]
    public async Task<ActionResult<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphFromOwnerDto>>>> AllFromOwner([FromQuery] GetAllOperationGraphFromOwnerFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка собственных графиков", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.AllFromOwnerAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
    }
}