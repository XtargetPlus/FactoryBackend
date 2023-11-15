using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles.TechnologicalProcesses;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.CUD;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;
using Shared.Dto.TechnologicalProcess.TechProcessItem.CUD;
using System.Security.Claims;

namespace Plan7.Controllers.TechnologicalProcesses;

/// <summary>
/// Контроллер разработчика тех процесса
/// </summary>
public class TpDeveloperController : BaseReactController<TpDeveloperController>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public TpDeveloperController(ILogger<TpDeveloperController> logger) : base(logger) { }

    /// <summary>
    /// Добавление операции тех процесса
    /// </summary>
    /// <param name="dto">Информация на добавление, где
    /// Priority - приоритет создаваемой ветки: 
    /// 1) n % 5 == 0, если ветка находится в Main
    /// 2) от n % 5 == 0 до (n + 5) % 5 == 0, если это ответвление
    /// MainTechProcessItemId - Id операции тех процесса из главной ветки (0, если добавление в главную ветку);
    /// OrdinalNumber - номер операции; OperationId - Id операции; TechProcessId - Id тех процесса
    /// Count - количество деталей; Note - примечание, null или текст</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.AddTpItem)]
    public async Task<IActionResult> AddTpItem(AddTechProcessItemDto dto, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление операции {OperationId} к тех процессу {TechProcessId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.OperationId, dto.TechProcessId);
        
        var id = await service.AddAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Добавление операции на станке к операции тех процесса
    /// </summary>
    /// <param name="dto">Информация на добавление, где
    /// EquipmentId - Id станка; TechProcessItemId - Id операции тех процесса; DebugTime - время наладки; LeadTime - время выполнения
    /// Note - примечание, возможен null</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.AddEquipmentOperation)]
    public async Task<IActionResult> AddEquipmentOperation(EquipmentOperationDto dto, IEquipmentOperationDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление станка {EquipmentId} к операции {TechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.EquipmentId, dto.TechProcessItemId);
        
        await service.AddAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Создание нового ответвления
    /// </summary>
    /// <param name="dto">Информация для добавления, где
    /// MainTechProcessItemId - Id операции из Main ветки, в которой создается ветка
    /// OrdinalNumber - номер операции; OperationId - Id операции; TechProcessId - Id тех процесса
    /// Count - количество деталей; Note - примечание, null или текст</param>
    /// <returns>Возвращает операцию глушилку или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.AddBranch)]
    public async Task<IActionResult> AddBranch(AddBranchWithItemDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление ветки для операции {TechProcessItemId} тех процесса {NewItemInformation.TechProcessId}",
            HttpContext.User.FindFirstValue("UserId"), dto.MainTechProcessItemId, dto.NewItemInformation.TechProcessId);
        
        var (techProcessItemId, branch) = await service.AddAsync(dto) ?? (0, 0);

        return service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings })
            : Ok(new { TechProcessItemId = techProcessItemId, Branch = branch });
    }

    /// <summary>
    /// Изменение операции тех процесса
    /// </summary>
    /// <param name="dto">Информация на изменение, где
    /// TechProcessItemId - Id редактируемой операции тех процесса; OrdinalNumber - номер операции; OperationId - Id операции;
    /// Count - количество деталей; Note - примечание, null или текст</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.ChangeTpItem)]
    public async Task<IActionResult> ChangeTpItem(ChangeTechProcessItemDto dto, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение операции {TechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessItemId);

        await service.ChangeAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение операции на станке операции тех процесса
    /// </summary>
    /// <param name="dto">Информация на изменение, где
    /// NewEquipmentId - Id нового станка операции тех процесса; EquipmentId - Id текущего станка операции тех процесса;
    /// TechProcessItemId - Id операции тех процесса, в котором все происходит; DebugTime - новая время наладки, LeadTime - новое время выполнения;
    /// Note - новое примечание, возможно null</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.ChangeEquipmentOperation)]
    public async Task<IActionResult> ChangeEquipmentOperation(ChangeEquipmentOperationDto dto, IEquipmentOperationDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение операции на станке {EquipmentId} - {TechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.EquipmentId, dto.TechProcessItemId);
        
        await service.ChangeAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение информации тех процесса
    /// </summary>
    /// <param name="dto">Информация на изменение, где
    /// TechProcessId - Id тех процесса; BlankTypeId - Id типа заготовки; MaterialId - Id материала; Rate - норма расхода</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.ChangeInfo)]
    public async Task<IActionResult> ChangeInfo(ChangeTechProcessData dto, ITpDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение данных тех процесса {TechProcessId}: Материал {MaterialId} - Тип заготовки {BlankTypeId} - {Rate}",
            HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.MaterialId, dto.BlankTypeId, dto.Rate);

        await service.ChangeTpDataInfoAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение статуса тех процесса
    /// </summary>
    /// <param name="dto">Информация для изменения статуса, где
    /// TechProcessStatuses - статус разработки тех процесса, относящиеся к технологам разработчикам;
    /// TechProcessId - Id тех процесса; Note - примечание или null</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.ChangeStatus)]
    public async Task<IActionResult> ChangeStatus(DeveloperChangeTechProcessStatusDto dto, ITpDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение статуса тех процесса {TechProcessId}", HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId);

        await service.ChangeTpStatusAsync(dto, (int)dto.TechProcessStatuses, int.Parse(HttpContext.User.FindFirstValue("UserId")!));

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операций из главной ветки в новое ответвление другой операции из главной ветки
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// MainTechProcessItemId - Id операции тех процесса из главной ветки, в новую ветку которой перемещаются операции из главной ветки
    /// MainItemsId - Список Id операций тех процесса, которые перемещаются в новую ветку</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.FromMainToNewBranch)]
    public async Task<IActionResult> FromMainToNewBranch(FromMainToNewBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {UserId}: Перемещение списка операций в новое ответвление операции {TechProcessItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.MainTechProcessItemId);
        
        await service.FromMainToNewBranchAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операций из главной ветки в существующее ответвление другой операции из главной ветки
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// TechProcessId - Id тех процесса, в котором это происходит вставка
    /// MainTechProcessItemId - Id операции тех процесса, в ветку которой перемещается операция тех процесса
    /// Branch - номер ветки, в которую перемещается операция тех процесса
    /// BeforeBranchItemId - Id операции тех процесса из ответвления, перед которой вставляется перемещаемся операция тех процесса
    /// CurrentTargetMainItemId - Id перемещаемой операции тех процесса из главной ветки</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.FromMainToBranch)]
    public async Task<IActionResult> FromMainToBranch(FromMainToBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Перемещение операции тех процесса {FormerMainTechProcessItem} в существующее ответвление операции {TechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.CurrentTargetMainItemId, dto.MainTechProcessItemId);
        
        await service.FromMainToBranchAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операций из ответвления в главную ветку
    /// </summary>
    /// <param name="dto">Информация для вставки, где 
    /// MainTechProcessItemId - Id операции из тех процесса, чью ветку перемещают
    /// Branch - перемещаемая ветка в главную ветку</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.GetBranchToMain)]
    public async Task<IActionResult> GetBranchToMain(BaseBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Перемещения ответвления из операции {TechProcessItemId} в главную ветку", 
            HttpContext.User.FindFirstValue("UserId"), dto.MainTechProcessItemId);
        
        await service.GetBranchToMainAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операции из ответвления в основную ветку
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// TechProcessId - Id тех процесса, в котором происходит операция
    /// BeforeMainItemId - Id операции тех процесса из главной ветки, перед которой должна встать операция из ответвления (передавать 0, если вставка в начало)
    /// CurrentTargetBranchItemId - Id перемещаемой операции из ответвления в главную ветку</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.GetBranchItemToMain)]
    public async Task<IActionResult> GetBranchItemToMain(GetBranchItemToMainDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Перемещения операции ответвления {TechProcessItemId} в главную ветку",
            HttpContext.User.FindFirstValue("UserId"), dto.CurrentTargetBranchItemId);
        
        await service.GetBranchItemToMainAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена местами операций тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены местами, где
    /// TechProcessId - Id тех процесса; FirstItemId - Id первой операции тех процесса, которая участвует в свапе;
    /// SecondItemId - Id второй операции тех процесса, которая участвует в свапе</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.SwapItems)]
    public async Task<IActionResult> SwapItems(SwapItemsDto dto, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Меняем местами операции {FirstItemId} и {SecondItemId} тех процесса {TechProcessId}",
            HttpContext.User.FindFirstValue("UserId"), dto.FirstItemId, dto.SecondItemId, dto.TechProcessId);

        await service.SwapAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена местами веток операции тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены местами, где
    /// MainTechProcessItemId - Id операции тех процесса из Main, в которой происходит свап веток;
    /// FirstBranch - номер первой ветки; SecondBranch - номер второй ветки</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.SwapBranches)]
    public async Task<IActionResult> SwapBranches(SwapBranchesDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Меняем местами операции ветки {FirstBranch} и {SecondBranch} тех процесса {MainTechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.FirstBranch, dto.SecondBranch, dto.MainTechProcessItemId);

        await service.SwapAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена местами операции на станках операции тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены местами, где
    /// TechProcessItemId - Id операции тех процесса, в которой меняются местами станки;
    /// FirstEquipmentId - Id первого станка; SecondEquipmentId - Id второго станка;</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.SwapEquipmentOperations)]
    public async Task<IActionResult> SwapEquipmentOperations(SwapEquipmentOperationDto dto, IEquipmentOperationDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Меняем местами операции на станках {FirstEquipmentId} и {SecondEquipmentId} операции {TechProcessItemId}", 
            HttpContext.User.FindFirstValue("UserId"), dto.FirstEquipmentId, dto.SecondEquipmentId, dto.TechProcessItemId); 
        
        await service.SwapAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка операции тех процесса между двумя другими
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// TechProcessId - Id тех процесса, в котором происходит вставка
    /// BeforeItemId - Id операции тех процесса, перед которой должна встать передвигаемая операция тех процесса (передавать 0, если вставка в начало)
    /// CurrentTargetItemId - Id передвигаемой операции тех процесса</param>
    /// <param name="service">Сервис операций тех процесса</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.InsertBetweenTechProcessItem)]
    public async Task<IActionResult> InsertBetweenTechProcessItem(InsertBetweenItemsDto dto, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка операции тех процесса между двумя другими", HttpContext.User.FindFirstValue("UserId"));
        
        await service.InsertBetweenAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();

    }

    /// <summary>
    /// Вставка операции на станке между операциями на станке
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// TechProcessItemId - Id операции тех процесса
    /// BeforePriority - приоритет станка, перед которой должен встать передвигаемый станок (передавать 0, если станок встает в самое начало)
    /// CurrentTargetPriority - текущий приоритет передвигаемого станка</param>
    /// <param name="service">Сервис операций на станках</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.InsertBetweenEquipmentOperation)]
    public async Task<IActionResult> InsertBetweenEquipmentOperation(InsertBetweenEquipmentOperation dto, IEquipmentOperationDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка операции на станке между двумя другими", HttpContext.User.FindFirstValue("UserId"));
        
        await service.InsertBetweenAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка ветки операции между другими ветками
    /// </summary>
    /// <param name="dto">Информация для вставки, где
    /// MainTechProcessItemId - Id операции тех процесса из главной ветки, в которой происходит вставка веток
    /// BeforeBranch - номер ветки, перед которой должна встать текущая ветка (передавать 0, если вставка в начало)
    /// CurrentTargetBranch - номер передвигаемой ветки</param>
    /// <param name="service">Сервис веток операций тех процесса</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.InsertBetweenBranch)]
    public async Task<IActionResult> InsertBetweenBranch(InsertBetweenBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка ветки между двумя другими", HttpContext.User.FindFirstValue("UserId"));
       
        await service.InsertBetweenAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена актуальности тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены актуальности, где
    /// TechProcessId - Id тех процесса, чья актуальность меняется; IsActual - true: актуален, false: нет;</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.ChangeActualTProcess)]
    public async Task<IActionResult> ChangeActualTProcess(ChangeTechProcessActualDto dto, ITpDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Смена видимости тех процесса {TechProcessId}-{IsVisibility}", HttpContext.User.FindFirstValue("UserId"), dto.TechProcessId, dto.IsActual);
        
        await service.ChangeActualTProcessAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Раскрытие операции тех процесса
    /// </summary>
    /// <param name="techProcessItemId">Id операции, которую нужно раскрыть</param>
    /// <returns>Операции, которые изменились, включая раскрытую</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.UncoverTpItemWithBranchesItems)]
    public async Task<ActionResult<List<GetTechProcessItemDto>>> UncoverTpItemWithBranchesItems(int techProcessItemId, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Раскрытие операции {TechProcessItemId}", HttpContext.User.FindFirstValue("UserId"), techProcessItemId);
        
        var tpItems = await service.UncoverWithBranchesItemsAsync(techProcessItemId);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok(tpItems);
    }

    /// <summary>
    /// Сокрытие или раскрытие ветки операции тех процесса
    /// </summary>
    /// <param name="dto">Информация для сокрытия или раскрытия ветки, где
    /// Visibility - true: видим, false: нет; MainTechProcessItemId - Id операции из Main, в которой это все происходит; Branch - номер ветки, который скрывается или расскрывается</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = TpDeveloperControllerRoles.HideOrUncoverBranch)]
    public async Task<IActionResult> HideOrUncoverBranch(HideOrUncoverBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Сокрытие или раскрытие ветки № {BranchNumber} операции тех процесса {TechProcessItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.Branch, dto.MainTechProcessItemId);
        
        await service.HideOrUncoverAsync(dto);

        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Сокрытие операции тех процесса
    /// </summary>
    /// <param name="techProcessItemId">Информация для сокрытия операции тех процесса</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpDelete("{techProcessItemId:int}")]
    [Authorize(Roles = TpDeveloperControllerRoles.HideTpItemWithBranchesItems)]
    public async Task<IActionResult> HideTpItemWithBranchesItems(int techProcessItemId, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Сокрытие операции {TechProcessItemId}", HttpContext.User.FindFirstValue("UserId"), techProcessItemId);
        
        await service.HideWithBranchesItemsAsync(techProcessItemId);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление операции тех процесса со всеми его ответвлениями
    /// </summary>
    /// <param name="techProcessItemId">Информация для удаления операций тех процесса</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpDelete("{techProcessItemId:int}")]
    [Authorize(Roles = TpDeveloperControllerRoles.DeleteTpItemWithBranchesItems)]
    public async Task<IActionResult> DeleteTpItemWithBranchesItems(int techProcessItemId, ITpItemDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаляет операцию {TechProcessItemId} тех процесса и все его ветки", HttpContext.User.FindFirstValue("UserId"), techProcessItemId);
        
        await service.DeleteWithBranchesItemsAsync(techProcessItemId);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление операции на станке операции тех процесса
    /// </summary>
    /// <param name="equipmentOperationId">Id операции тех процесса на станке</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{equipmentOperationId:int}")]
    [Authorize(Roles = TpDeveloperControllerRoles.DeleteEquipmentOperation)]
    public async Task<IActionResult> DeleteEquipmentOperation(int equipmentOperationId, IEquipmentOperationDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление операции на станке {equipmentOperationId}", 
            HttpContext.User.FindFirstValue("UserId"), equipmentOperationId);
        
        await service.DeleteAsync(equipmentOperationId);

        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok();
    }

    /// <summary>
    /// Удаление ветки операции тех процесса
    /// </summary>
    /// <param name="dto">Информация об ветки, где
    /// MainTechProcessItemId - Id операции тех процесса, в которой происходит удаление ветки,
    /// Branch - номер удаляемой ветки</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete]
    [Authorize(Roles = TpDeveloperControllerRoles.DeleteBranchWithItems)]
    public async Task<IActionResult> DeleteBranchWithItems([FromQuery] BaseBranchDto dto, ITpBranchDeveloperService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление ветки {Branch} операции на станке {MainTechProcessItemId}",
            HttpContext.User.FindFirstValue("UserId"), dto.Branch, dto.MainTechProcessItemId);
        
        await service.DeleteWithItemsAsync(dto);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok();
    }
}
