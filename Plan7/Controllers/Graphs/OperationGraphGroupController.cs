using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Graphs;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.CUD;
using Shared.Dto.Graph.Access;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;

namespace Plan7.Controllers.Graphs;

public class OperationGraphGroupController : BaseReactController<OperationGraphGroupController>
{
    private readonly IOperationGraphGroupService _service;

    public OperationGraphGroupController(ILogger<OperationGraphGroupController> logger, IOperationGraphGroupService service) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Создание группы графиков
    /// </summary>
    /// <param name="operationGraphsId">Список Id графиков. Минимальное количество - 2</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphGroupControllerRoles.CreateGroup)]
    public async Task<IActionResult> CreateGroup(List<int> operationGraphsId)
    {
        _logger.LogInformation("Пользователь {userId}: Создание группы графиков: {graphsId}",
            HttpContext.User.FindFirstValue("UserId"), string.Join(", ", operationGraphsId));

        await _service.CreateGroupAsync(operationGraphsId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Добавление графиков в существующую группу
    /// </summary>
    /// <param name="dto">Информация для добавление графиков в группу</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphGroupControllerRoles.AddGraphs)]
    public async Task<IActionResult> AddGraphs(AddGraphsInGroupDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление графиков {graphs} группу графика {mainGraph}",
            HttpContext.User.FindFirstValue("UserId"), string.Join(", ", dto.GraphsId), dto.MainGraphId);

        await _service.AddGraphsAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }
    
    /// <summary>
    /// Копирование графика или группы графиков
    /// </summary>
    /// <param name="dto">В GraphId всегда передавать MainGraphId, если это группа графиков</param>
    /// <returns>Ok (id созданного графика (mainGraphId)) или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Copy)]
    public async Task<IActionResult> Copy(CopyGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Копирование графика {graphId}, подразделение {subdivisionId}, план {planCount}, дата графика {graphDate}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphId, dto.SubdivisionId, dto.PlanCount, dto.GraphDate);

        var id = await _service.CopyAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Редактирование графика в группе
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphGroupControllerRoles.ChangeGraphInfo)]
    public async Task<IActionResult> ChangeGraphInfo(ChangeGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Редактирование графика в группе {graphId}, количество - {planCount}, дата графика - {graphDate}, подразделение - {subdivisionId}, примечение - {note}",
            HttpContext.User.FindFirstValue("UserId"), dto.OperationGraphId, dto.PlanCount, dto.GraphDate, dto.SubdivisionId, dto.Note);

        await _service.ChangeGraphInfoAsync(dto);   

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Заморозка (пауза) группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <returns></returns>
    [HttpPut("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Freeze)]
    public async Task<IActionResult> Freeze(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Остановка разработки графика {mainGraphId} (заморозка / пауза)",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.FreezeOrUnAsync(mainGraphId, true);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Восстановление работы над группой графиков (разморозка группы)
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// /// <returns></returns>
    [HttpPut("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Unfreeze)]
    public async Task<IActionResult> Unfreeze(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Возврат в работу графика {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.FreezeOrUnAsync(mainGraphId, false);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Перегенерация группы графиков (приведение базовому виду, как после создания группы)
    /// </summary>
    /// <param name="mainGraphId">Id главного графика в группе</param>
    /// <returns></returns>
    [HttpPut("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Regeneration)]
    public async Task<IActionResult> Regeneration(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Перегенрация группы графиков, где main - {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.RegenerationAsync(mainGraphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена местами графиков
    /// </summary>
    /// <param name="dto">Информация для смены местами графиков (учитываются все графики, группы и не группы). В id передавать mainGraphId</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Swap)]
    public async Task<IActionResult> Swap(SwapGraphDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Смена местами группы {targetPriority} и {sourcePriority}",
            HttpContext.User.FindFirstValue("UserId"), dto.TargetGroupPriority, dto.TargetGroupPriority);

        await _service.SwapAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Подтверждение группы графиков, вместе с их деталями
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Confirm)]
    public async Task<IActionResult> Confirm(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Подтверждение группы графиков, где main - {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.ConfirmAsync(mainGraphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Снятие подтверждения с группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика группы</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Unconfirm)]
    public async Task<IActionResult> Unconfirm(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Снятие подтверждения с группы графиков, где main - {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.UnconfirmAsync(mainGraphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns> 
    [HttpDelete("{mainGraphId:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int mainGraphId)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление группы графика, где главный график - {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), mainGraphId);

        await _service.DeleteAsync(mainGraphId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаленние графиков из группы с возможностью выборы типа удаления
    /// </summary>
    /// <param name="dto">Информация для удаления. Где DeleteType: 0 - Полное удаление; 1 - Перенос в новую группу; 2 - Перенос в одиночные записи.</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpDelete]
    [Authorize(Roles = OperationGraphGroupControllerRoles.DeleteGraphs)]
    public async Task<IActionResult> DeleteGraphs(DeleteGraphsInGroupDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление графиков {graphs} из группы {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), string.Join(", ", dto.GraphsId), dto.MainGraphId);

        await _service.DeleteGraphsAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получение группы графиков
    /// </summary>
    /// <param name="priority">Приоритет группы</param>
    /// <returns>Ok (группа) или BadRequest (ошибки и предупреждения)</returns>
    [HttpGet("{priority:int}")]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Group)]
    public async Task<ActionResult<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>>> Group(int priority)
    {
        _logger.LogInformation("Пользователь {userId}: Получение группы графиков, где приоритет группы - {priority}",
            HttpContext.User.FindFirstValue("UserId"), priority);

        var result = await _service.GroupAsync(priority, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }

    /// <summary>
    /// Получаем информацию о группе графиков
    /// </summary>
    /// <param name="filters">Фильтры выборки, где grahId - id main графика группы</param>
    /// <returns>Список открытых графиков с их деталями и операциями деталей</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphGroupControllerRoles.Open)]
    public async Task<ActionResult<OpenOperationGraphGroupDto>> Open([FromQuery] OpenOperationGraphFilters filters, [FromServices] IServiceProvider provider)
    {
        _logger.LogInformation("Пользователь {userId}: Открытие группы графиков, где main - {mainGraphId}",
            HttpContext.User.FindFirstValue("UserId"), filters.GraphId);

        var result = await _service.OpenAsync(filters, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"), provider.GetService<IOperationGraphService>()!);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }
}