using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.TechnologicalProcessInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Enums;
using Shared.BasicStructuresExtensions;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;
using ServiceLayer.TechnologicalProcesses.Services.SubClasses;
using Shared.Dto.TechnologicalProcess.TechProcessItem.CUD;
using AutoMapper;
using BizLayer.Repositories.TechnologicalProcessR;

namespace ServiceLayer.TechnologicalProcesses.Services.Developer;

/*
 * Ответвления операции технического процесса.
 * 
 * У каждой операции есть свой уникальный порядковый номер в ветке, в которая она находится: 1, 2, 3, 4. Это номер порядка выполнения операций.
 * Так же у каждой операции есть свой string номер операции: "005", "010", "015 x", "120" и тд. Он уникальный для всех операций технического процесса.
 * Если станок, на котором производится текущая операция сломан, необходимо предусматривать альтернативные пути выполнения - ответвления операции.
 * 
 * Ветка в данном случае, это когда есть основная операция "005" (она находится в основной ветке), но вместо нее можно выполнить 3 операции: "006", 007", 008"
 * У одной операции может быть несколько различных веток, то есть вместо выполнения операции "005" мы можем выполнить операции из ветки 1, ветки 2 или ветки 3
 * Так же у веток есть свои приоритеты выполнения - ветка 1 в высшем приоритете, чем ветка 2 и тд
 * 
 * Как выстраиваются ветки? За счет приоритетов.
 * 
 * У операций main ветки приоритеты всех операций кратны 5 - "005": 5; "010": 10, "015": 15 и тд. У первой операции технического процесса приоритет 5
 * Если мы хоти создать ветку 1 для операции "005":
 * 1) Создается операция заглушка, ее необходимо сразу отредактировать, ее приоритет - 6 (номер ветки так же 6)
 * 2) Далее мы заполняем ветку операциями - у всех операций текущей ветки будет приоритет 6
 * 3) Мы создаем новую ветку - приоритет операций этой ветки 7
 * ...
 * n) После создания ветки с номером 9 возможность создания веток для операции "005" исчезает, пока не удалится хотя бы одна ветка
 * 
 * Количество веток на 1 операцию - 4 (от n % 5 == 0 до (n + 5) % 5 == 0 не включительно)
*/

/// <summary>
/// Сервис операций тех процесса для разработчиков тех процессов
/// </summary>
public class TpItemDeveloperService : ErrorsMapper, ITpItemDeveloperService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcessItem> _repository;
    private readonly IMapper _dataMapper;

    public TpItemDeveloperService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление операции технического процесса
    /// </summary>
    /// <param name="dto">Информация на добавление и в какую ветку ее добавлять</param>
    /// <returns>Id добавленной операции или null (ошибки и предупреждения)</returns>
    public async Task<int?> AddAsync(AddTechProcessItemDto dto)
    {
        var isMainOperation = dto.Priority % 5 == 0;

        TechnologicalProcessItem? techProcessItem = new()
        {
            Number = _repository
                .GetAll(tpi => tpi.TechnologicalProcessId == dto.TechProcessId && (isMainOperation ? tpi.Priority % 5 == 0 : tpi.Priority == dto.Priority))?
                .OrderBy(tpi => tpi.Number)
                .LastOrDefault()?.Number + 1 ?? 1,
            OperationNumber = dto.OperationNumber,
            Priority = _repository
                .GetAll(tpi => tpi.TechnologicalProcessId == dto.TechProcessId && (isMainOperation ? tpi.Priority % 5 == 0 : tpi.Priority == dto.Priority))?
                .OrderBy(tpi => tpi.Number)
                .LastOrDefault()?.Priority + (isMainOperation ? 5 : 0) ?? (isMainOperation ? 5 : dto.Priority),
            Count = dto.Count,
            Show = true,
            OperationId = dto.OperationId,
            TechnologicalProcessId = dto.TechProcessId,
            MainTechnologicalProcessItemId = dto.MainTechProcessItemId != 0 ? dto.MainTechProcessItemId : null,
            Note = dto.Note,
        };

        techProcessItem = await _context.AddWithValidationsAndSaveAsync(techProcessItem, this);
        return techProcessItem?.Id;
    }

    /// <summary>
    /// Изменение операции технического процесса
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns></returns>
    public async Task ChangeAsync(ChangeTechProcessItemDto dto)
    {
        var techProcessItem = await TechProcessItemSimpleRead.GetAsync(_repository, dto.TechProcessItemId, this);
        if (HasErrors) return;

        if (techProcessItem!.Count != dto.Count) techProcessItem.Count = dto.Count;
        if (techProcessItem.OperationId != dto.OperationId) techProcessItem.OperationId = dto.OperationId;
        if (dto.Note != null && techProcessItem.Note != dto.Note) techProcessItem.Note = dto.Note;
        if (!string.IsNullOrEmpty(dto.OperationNumber) && techProcessItem.OperationNumber != dto.OperationNumber)
        {
            if (_repository.FindFirstAsync(filter: tpi => tpi.OperationNumber == dto.OperationNumber 
                                                          && tpi.TechnologicalProcessId == techProcessItem.TechnologicalProcessId
                                                          && tpi.Id != techProcessItem.Id) != null)
            {
                AddErrors("Попытка изменить номер операции тех процесса на уже существующий");
                return;
            }
            techProcessItem.OperationNumber = dto.OperationNumber;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Меняем местами операции технического процесса и их ветки с операциями на этих ветках
    /// </summary>
    /// <param name="dto">Информация об операциях технического процесса, которые нужно поменять местами</param>
    /// <returns></returns>
    public async Task SwapAsync(SwapItemsDto dto)
    {
        TpReadonlyService techProcessReadonlyService = new(_context, _dataMapper);

        var firstItem = await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.FirstItemId, this);
        var secondItem = await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.SecondItemId, this);

        var firstBranchNumbers = await techProcessReadonlyService.GetNumberOfBranchesAsync(dto.FirstItemId);
        if (firstBranchNumbers is null)
            AddErrors("Не удалось получить список веток первой операции тех процесса");

        var secondBranchNumbers = await techProcessReadonlyService.GetNumberOfBranchesAsync(dto.SecondItemId);
        if (secondBranchNumbers is null)
            AddErrors("Не удалось получить список веток второй операции тех процесса");

        if (techProcessReadonlyService.HasErrors)
            AddErrors(string.Join("\n", techProcessReadonlyService.Errors));
        if (HasErrors) return;
        
        (firstItem!.Priority, secondItem!.Priority) = (secondItem.Priority, firstItem.Priority);
        (firstItem.Number, secondItem.Number) = (secondItem.Number, firstItem.Number);

        TechProcessItemRepository subFunc = new(_repository, this);

        await subFunc.ChangeBranchesOperationsPriorityAsync(firstBranchNumbers!, firstItem.Id, firstItem.Priority, secondItem.Priority);
        await subFunc.ChangeBranchesOperationsPriorityAsync(secondBranchNumbers!, secondItem.Id, secondItem.Priority, firstItem.Priority);
        if (HasErrors) return;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка операции тех процесса между двумя другими с перерасчетом всех приоритетов и номеров затронутых операций тех процесса
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenAsync(InsertBetweenItemsDto dto)
    {
        if (dto.BeforeItemId == dto.CurrentTargetItemId)
            AddErrors("Операции тех процесса №1 и №2 совпадают");
        
        var firstItem = dto.BeforeItemId != 0 ? await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.BeforeItemId, this) : null;
        var secondItem = await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.CurrentTargetItemId, this);
        if (HasErrors) return;

        if (firstItem is not null && firstItem.MainTechnologicalProcessItemId != secondItem!.MainTechnologicalProcessItemId)
            AddErrors("Операции принадлежат разным веткам");
        if (HasErrors) return;

        var variables = new InsertBetweenItemsVariables(firstItem, secondItem!);
        TechProcessItemHardRead hardRead = new(_repository, this);
        TechProcessItemRepository subFunc = new(_repository, this);

        var changedItems = variables.IsMainBranch switch
        {
            true => await hardRead.GetMainItemsWithRangeForNumberAsync(variables.OldNumber, variables.NewNumber, dto.TechProcessId),
            false => await hardRead.GetBranchItemsWithRangeForNumberAsync(
                variables.OldNumber, variables.NewNumber, dto.TechProcessId, secondItem!.MainTechnologicalProcessItemId!.Value, variables.NewPriority)
        };
        if (changedItems.IsNullOrEmpty()) return;

        if (variables.IsMainBranch)
        {
            var ownedItems = await hardRead.GetBranchesItemsWithoutValidationAsync(secondItem!.TechnologicalProcessId, secondItem.Id, QueryFilterOptions.TurnOff);
            ownedItems?.ForEach(tpi => tpi.Priority = variables.NewPriority + tpi.Priority % 5);

            var readonlyService = new TpReadonlyService(_context, _dataMapper);
            foreach (var item in changedItems!)
            {
                var aliensItems = await hardRead.GetBranchesItemsAsync(
                    item.Id,
                    await readonlyService.GetNumberOfBranchesAsync(item.Id, QueryFilterOptions.TurnOff) ?? new(), 
                    QueryFilterOptions.TurnOff);

                aliensItems?.ForEach(item => item.Priority += variables.IsOldNumberBigger ? 5 : -5);
            }
        }
        if (HasErrors) return;

        _ = subFunc.ChangeNumberAndPriority(changedItems!, variables.IsOldNumberBigger, variables.IsMainBranch); 

        secondItem!.Number = variables.NewNumber;
        secondItem.Priority = variables.NewPriority;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Раскрываем операцию с учетом ее ответвлений, если это операция из главной ветки.
    /// Так же изменяются номера и приоритеты всех операций, которые находятся выше, дабы сохранить правильную последовательность выполняемых операций тех процесс
    /// </summary>
    /// <param name="techProcessItemId">Id операции тех процесса, которую нужно раскрыть</param>
    /// <returns>Возвращаем список операций, чьи номера и приоритеты были изменены включая операцию tpItemId или null (ошибки с предупреждениями)</returns>
    public async Task<List<GetTechProcessItemDto>?> UncoverWithBranchesItemsAsync(int techProcessItemId)
    {
        var techProcessItem = await TechProcessItemSimpleRead.GetWithIncludeOperationAsync(_repository, techProcessItemId, QueryFilterOptions.TurnOff, this);
        if (HasErrors) return null;

        TechProcessItemHardRead hardRead = new(_repository, this);

        // Учитываем что раскрытие может происходить как в главной ветке, так и в ответвлении
        if (techProcessItem!.Priority % 5 == 0)
        {   
            var ownedItems = await hardRead.GetBranchesItemsWithoutValidationAsync(techProcessItem.TechnologicalProcessId, techProcessItemId, QueryFilterOptions.TurnOff);
            ownedItems!.ForEach(tpi => tpi.Show = true);
        }
        if (HasErrors) return null;

        techProcessItem.Show = true;
        
        await RecalculationAfterHideOrUncoverAsync(techProcessItem);

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return null;

        List<GetTechProcessItemDto> result = new()
        {
            new GetTechProcessItemDto
            {
                Id = techProcessItemId,
                Number = techProcessItem.Number,
                OperationNumber = techProcessItem.OperationNumber,
                ShortName = techProcessItem.Operation?.ShortName ?? "",
                FullName = techProcessItem.Operation?.FullName ?? "",
                Priority = techProcessItem.Priority,
                Note = techProcessItem.Note
            }
        };

        return result;
    }

    /// <summary>
    /// Удаление операции технического процесса
    /// </summary>
    /// <param name="techProcessItemId">Id операции, которую нужно удалить</param>
    /// <returns></returns>
    public async Task HideWithBranchesItemsAsync(int techProcessItemId)
    {
        var techProcessItem = await TechProcessItemSimpleRead.GetAsync(_repository, techProcessItemId, this);
        if (HasErrors) return;

        var hidedItems = await GetDeleteOrHideItemsAsync(techProcessItem!);

        await TechProcessBranchValidations.HideItemsValidationAsync(hidedItems, _context, this);

        if (HasErrors) return;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        hidedItems.ForEach(tpi => tpi.Show = false);

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        await RecalculationAfterDeleteOrHideAsync(techProcessItem!);
        await RecalculationAfterHideOrUncoverAsync(techProcessItem!);

        if (hidedItems.Count > 1)
        {
            await _context.SaveChangesWithValidationsAsync(this);
            if (HasErrors) return;
        }

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Удаление операции тех процесса
    /// </summary>
    /// <param name="techProcessItemId">Id</param>
    /// <returns></returns>
    public async Task DeleteWithBranchesItemsAsync(int techProcessItemId)
    {
        var techProcessItem = await TechProcessItemSimpleRead.GetAsync(_repository, techProcessItemId, this);
        if (HasErrors) return;
        
        var deletedItems = await GetDeleteOrHideItemsAsync(techProcessItem!);
        if (HasErrors) return;

        _repository.Remove(deletedItems);

        await RecalculationAfterDeleteOrHideAsync(techProcessItem!);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="techProcessItem"></param>
    /// <returns></returns>
    private async Task<List<TechnologicalProcessItem>> GetDeleteOrHideItemsAsync(TechnologicalProcessItem techProcessItem)
    {
        List<TechnologicalProcessItem> result = new() { techProcessItem };
        if (techProcessItem.Priority % 5 == 0)
            result.AddRange(await new TechProcessItemHardRead(_repository, this)
                .GetBranchesItemsWithoutValidationAsync(techProcessItem.TechnologicalProcessId, techProcessItem.Id, QueryFilterOptions.TurnOff) ?? new());
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="techProcessItem"></param>
    /// <returns></returns>
    private async Task RecalculationAfterDeleteOrHideAsync(TechnologicalProcessItem techProcessItem)
    {
        if (techProcessItem.MainTechnologicalProcessItemId != null)
        {
            var branchItems = await _repository.GetAllAsync(
                    filter: tpi => tpi.MainTechnologicalProcessItemId == techProcessItem.MainTechnologicalProcessItemId
                                   && tpi.Priority == techProcessItem.Priority
                                   && tpi.Number > techProcessItem.Number,
                    trackingOptions: TrackingOptions.WithTracking,
                    addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff));

            branchItems?.ForEach(item => item.Number--);
        }
        else
        {
            var items = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessItem.TechnologicalProcessId
                           && tpi.Priority > techProcessItem.Priority
                           && (tpi.MainTechnologicalProcessItemId == null || tpi.MainTechnologicalProcessItemId != techProcessItem.Id),
            orderBy: o => o.OrderBy(tpi => tpi.Priority),
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff)); 
            
            if (items.IsNullOrEmpty()) return;

            foreach (var item in items!)
            {
                item.Priority -= 5;
                if (item.MainTechnologicalProcessItemId == null)
                    item.Number--;
            }
        }
    }

    private async Task RecalculationAfterHideOrUncoverAsync(TechnologicalProcessItem techProcessItem)
    {
        if (techProcessItem.MainTechnologicalProcessItemId != null)
        {
            var branchItems = await _repository.GetAllAsync(
                filter: tpi => tpi.MainTechnologicalProcessItemId == techProcessItem.MainTechnologicalProcessItemId
                               && tpi.Priority == techProcessItem.Priority,
                trackingOptions: TrackingOptions.WithTracking);

            if (branchItems.IsNullOrEmpty()) return;

            for (var i = 0; i < branchItems!.Count; i++)
                branchItems[i].Number = i + 1;
        }
        else
        {
            var items = await _repository.GetAllAsync(
                filter: tpi => tpi.TechnologicalProcessId == techProcessItem.TechnologicalProcessId,
                orderBy: o => o.OrderBy(tpi => tpi.Priority),
                trackingOptions: TrackingOptions.WithTracking);
            
            if (items.IsNullOrEmpty()) return;

            var lastMainPriority = 0;
            var lastMainNumber = 1;
            for (var i = 0; i < items!.Count; i++)
            {
                if (items[i].Priority % 5 > 0)
                {
                    items[i].Priority = lastMainPriority + items[i].Priority % 5;
                }
                else
                {
                    items[i].Priority = lastMainPriority + 5;
                    lastMainPriority = items[i].Priority;
                }

                if (items[i].MainTechnologicalProcessItemId != null) continue;
                
                items[i].Number = lastMainNumber;
                lastMainNumber++;
            }
        }
    }

    public void Dispose() => _context.Dispose();
}
