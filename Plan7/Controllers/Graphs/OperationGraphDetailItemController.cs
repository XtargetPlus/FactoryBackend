using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Graphs;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.Item;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.BranchesItems;
using Shared.Dto.Graph.Read.Open;

namespace Plan7.Controllers.Graphs;

/// <summary>
/// 
/// </summary>
public class OperationGraphDetailItemController : BaseReactController<OperationGraphDetailItemController>
{
    private readonly IOperationGraphDetailItemService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OperationGraphDetailItemController(ILogger<OperationGraphDetailItemController> logger, IOperationGraphDetailItemService service) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление операции тех процесса в деталь графика. Добавлени происходит в самый конец
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.Add)]
    public async Task<IActionResult> Add(GraphDetailItemDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление в деталь графика {graphDetailId} операцию тек процесса {techProcessItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphDetailId, dto.TechProcessItemId);

        await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Добавление операции тех процесса в конец блока операций детали графика (где priority % 5 != 0)
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.AddToBlock)]
    public async Task<IActionResult> AddToBlock(AddToBlockGraphDetailItemDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление операции тех процесса {techProcessId} в деталь в {graphDetailId} в блок с приоритетом {priority}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessItemId, dto.GraphDetailId, dto.Priority);

        await _service.AddToBlockAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Временная конечная точка, добавление фактического количества деталей, которые прошли операцию детали графика
    /// </summary>
    /// <param name="dto">Информация для добавления фактического количества</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.AddFactCount)]
    public async Task<IActionResult> AddFactCount(AddFactCountDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление фактического количества деталей ({factCount}), которые прошли операцию {graphDetailItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.FactCount, dto.GraphDetailItemId);

        await _service.AddFactCountAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена ветки для операции или блока операций детали графика
    /// </summary>
    /// <param name="dto">Информация для смены ветки</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.SubstitutionToBranch)]
    public async Task<IActionResult> SubstitutionToBranch(SubstitutionToBranchDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Замена операций ветки {oldDetailItemPriority} на операции другой ветки {NewDetailItemPriority}, детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), dto.OldDetailItemPriority, dto.NewDetailItemPriority, dto.GraphDetailId);

        await _service.SubstitutionToBranchAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Свап блоков операций детали графика
    /// </summary>
    /// <param name="dto">Информация для свапа блоков, priority - приоритет операций тех процесса в блоке</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.SwapBlocks)]
    public async Task<IActionResult> SwapBlocks(SwapGraphDetailItemBlocksDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Свап блока операций графика с приориетами {targetItemPriority} и {sourceItemPriority} детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), dto.TargetItemPriority, dto.SourceItemPriority, dto.GraphDetailId);

        await _service.SwapBlocksAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Свап операций детали графика внутри блока операций
    /// </summary>
    /// <param name="dto">Информация для свапа</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.SwapInBlock)]
    public async Task<IActionResult> SwapInBlock(SwapGraphDetailItemsInBlockDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Свап операций детали внутри блока: {targetItemId} -> {sourceItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.TargetDetailItemId, dto.SourceDetailItemId);

        await _service.SwapInBlockAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка блока операций детали графика между двумя другими блоками
    /// </summary>
    /// <param name="dto">Информация для вставкию. NewFirstItemNumber - новый порядковый номер для первой операции пермещаемого блока</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.InsertBetweenBlocks)]
    public async Task<IActionResult> InsertBetweenBlocks(GraphDetailItemInsertBetweenBlocksDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка блока операций {targetItemPriority}, детали графика {graphDetailId}, между другими - новый номер первой операции блока {newFirstItemNumber}",
            HttpContext.User.FindFirstValue("UserId"), dto.TargetItemPriority, dto.GraphDetailId, dto.NewFirstItemNumber);

        await _service.InsertBetweenBlocksAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операции детали графика между двумя другими в пределах одного блока
    /// </summary>
    /// <param name="dto">Информация для вставки между</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.InsertBetweenItemsInBlock)]
    public async Task<IActionResult> InsertBetweenItemsInBlock(InsertBetweenGraphDetailItemInBlockDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка операции {targetItemNumber}, детали {graphDetailId} между другими в пределах блока - новый номер операции {newItemNumber}",
            HttpContext.User.FindFirstValue("UserId"), dto.TargetItemNumber, dto.GraphDetailId, dto.NewItemNumber);

        await _service.InsertBetweenItemsInBlockAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление операции детали графика
    /// </summary>
    /// <param name="graphDetailItemId">Id операции детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpDelete]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int graphDetailItemId)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление операции детали графика {graphDetailItemId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailItemId);

        await _service.DeleteAsync(graphDetailItemId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получение операции детали графика
    /// </summary>
    /// <param name="graphDetailItemId">Id операции</param>
    /// <returns>Ok (операция) или BadRequest (ошибки и предупреждения)</returns>
    [HttpGet("{graphDetailItemId:int}")]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.ById)]
    public async Task<ActionResult<InterimGraphDetailItemDto>> ById(int graphDetailItemId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение операции детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailItemId);

        var result = await _service.ByIdAsync(graphDetailItemId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }

    /// <summary>
    /// Получение списка операций детали графика
    /// </summary>
    /// <param name="dto">Информация для получения списка операций</param>
    /// <returns>Список операций</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.All)]
    public async Task<ActionResult<GraphDetailItemHigherDto>> All([FromQuery] GetAllDetailItemsDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение операция детали графика {graphDetailId} и тех процесс которого {techProcessId}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphDetailId, dto.TechProcessId);

        return Ok(await _service.AllAsync(dto));
    }

    /// <summary>
    /// Получаем список операций тех процесса, которых еще нет в списке операций деталий графика, так же учитывается, что некоторые операции могут быть заменены на ответвления
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций тех процесса, которые можно добавить</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.AllToAddToEndOfMain)]
    public async Task<ActionResult<List<GetAllToAddToEndDto>>> AllToAddToEndOfMain([FromQuery] AddToEndOfMainInfoDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка операций тех процесса {techProcessId} для детали {graphDetailId}, которые можно добавить в конец списка, " +
                               "с учетом того, что операции из main могут быть заменены на ответвления",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.GraphDetailId);

        return Ok(await _service.AllToAddToEndOfMainAsync(dto));
    }

    /// <summary>
    /// Получение списка операций ветки тех процесса, которые можно добавить в блок операций детали графика
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций тех процесса, которые можно добавить</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.AllToAddToEndOfBranch)]
    public async Task<ActionResult<List<GetAllBranchesItemsDto>?>> AllToAddToEndOfBranch([FromQuery] GetAllToAddToEndOfBranchDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка операций тех процесса {techProcessId} ветки {branch} для детали {graphDetailId}, которые можно добавить в конец блока ответвления",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.Priority, dto.GraphDetailId);

        return Ok(await _service.AllToAddToEndOfBranchAsync(dto));
    }

    /// <summary>
    /// Получение списка операций ответвления, с учетом того, что передаваться может приоритет ветки (main включительно)
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций ответвления</returns>
    [HttpGet]
    [Authorize(Roles = OperationGraphDetailItemControllerRoles.AllBranchesItems)]
    public async Task<ActionResult<List<GetAllBranchesItemsDto>>> AllBranchesItems([FromQuery] BranchesItemsInfoDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка веток с их операциями, где тех процесс: {techProcessId}, а приоритет переданной операции {priority}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.Priority);
        
        return Ok(await _service.AllBranchesItemsAsync(dto));
    }
}