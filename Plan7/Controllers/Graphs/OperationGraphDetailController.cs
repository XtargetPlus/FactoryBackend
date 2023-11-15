using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.Graphs;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.Detail;
using System.Security.Claims;
using DB.Model.UserInfo.RoleInfo;
using Shared.Dto.Graph.Read.Open;

namespace Plan7.Controllers.Graphs;

/// <summary>
/// 
/// </summary>
public class OperationGraphDetailController : BaseReactController<OperationGraphDetailController>
{
    private readonly IOperationGraphDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OperationGraphDetailController(ILogger<OperationGraphDetailController> logger, IOperationGraphDetailService service) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление детали в график
    /// </summary>
    /// <param name="dto">Информация для добавления. Usability указывать обязательно</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailControllerRoles.Add)]
    public async Task<IActionResult> Add(AddGraphDetailDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление детали {detailId} в график {graphId} с применяемостью {usability}",
            HttpContext.User.FindFirstValue("UserId"), dto.DetailId, dto.GraphId, dto.Usability);

        await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Временная конечная точка для добавления СГД к детали
    /// </summary>
    /// <param name="dto">Информация для добавления СГД</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailControllerRoles.AddFinishedGoodsInventory)]
    public async Task<IActionResult> AddFinishedGoodsInventory(AddFinishedGoodsInventoryDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление СГД в количество {finishedGoodsInventory} для детали {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), dto.FinishedGoodsInventory, dto.GraphDetailId);

        await _service.AddFinishedGoodsInventoryAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Временная конечная точка для добавления потока в деталь
    /// </summary>
    /// <param name="dto">Информация для добавления потока</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPost]
    [Authorize(Roles = OperationGraphDetailControllerRoles.AddCountInStream)]
    public async Task<IActionResult> AddCountInStream(AddCountInStreamDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление потока в количество {countInStream} для детали {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), dto.CountInStream, dto.GraphDetailId);

        await _service.AddCountInStreamAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение тех процесса детали графиков
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailControllerRoles.ChangeTechProcess)]
    public async Task<IActionResult> ChangeTechProcess(ChangeTechProcessDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение тех процесса детали графика {graphDetailId} на {techProcessId}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphDetailId, dto.TechProcessId);

        await _service.ChangeTechProcessAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение применяемости для детали графика
    /// </summary>
    /// <param name="dto">Информация для изменения применяемости</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailControllerRoles.ChangeUsability)]
    public async Task<IActionResult> ChangeUsability(ChangeUsabilityDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение применяемости для детали графика {graphDetailId} - {usability}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphDetailId, dto.Usability);

        await _service.ChangeUsabilityAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Подтверждение детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.Confirm)]
    public async Task<IActionResult> Confirm(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Подтверждение детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        await _service.ConfirmAsync(graphDetailId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Снятие подтверждения детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.Unconfirm)]
    public async Task<IActionResult> Unconfirm(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Снятие подтвреждения с детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        await _service.UnconfirmAsync(graphDetailId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Скрыть деталь графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.HideAndUncover)]
    public async Task<IActionResult> Hide(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Скрытие детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        await _service.HideOrUncoverAsync(graphDetailId, false);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Расскрыть деталь графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.HideAndUncover)]
    public async Task<IActionResult> Uncover(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Расскрытие детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        await _service.HideOrUncoverAsync(graphDetailId, true);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Свап деталей графика
    /// </summary>
    /// <param name="dto">Информация для свапа, где TargetPositionNumber и SourcePositionNumber это PositionNumberToDisplay</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailControllerRoles.Swap)]
    public async Task<IActionResult> Swap(SwapGraphDetailsDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Свап внутри графика {graphId}, где номер первой детали: {targetPositionNumber}" +
                               "а номер второй детали: {sourcePositionNumber}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphId, dto.TargetPositionNumber, dto.SourcePositionNumber);

        await _service.SwapAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка детали между двумя деталями
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpPut]
    [Authorize(Roles = OperationGraphDetailControllerRoles.InsertBetween)]
    public async Task<IActionResult> InsertBetween(InsertBetweenGraphDetailsDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка детали графика {graphId}, с порядковым номер {targetPositionNumber}, между двумя деталями {newPositionNumber}",
            HttpContext.User.FindFirstValue("UserId"), dto.GraphId, dto.TargetPositionNumber, dto.NewPositionNumber);

        await _service.InsertBetweenAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление детали графика (вместе с его составом). Удаление только в форме - С повторами
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok или BadRequest (ошибки и предупреждения)</returns>
    [HttpDelete("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        await _service.DeleteAsync(graphDetailId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Получение детали по id
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Ok (Деталь) или BadRequest (ошибки и предупреждения)</returns>
    [HttpGet("{graphDetailId:int}")]
    [Authorize(Roles = OperationGraphDetailControllerRoles.ById)]
    public async Task<ActionResult<GraphDetailDto>> ById(int graphDetailId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение детали графика {graphDetailId}",
            HttpContext.User.FindFirstValue("UserId"), graphDetailId);

        var result = await _service.ByIdAsync(graphDetailId);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(result);
    }
}