using AutoMapper;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using DB;
using ServiceLayer.Graphs.Services.Interfaces;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using DatabaseLayer.DatabaseChange;
using Shared.Dto.Graph.Read;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using Shared.BasicStructuresExtensions;
using Shared.Dto.Graph.Read.BranchesItems;
using BizLayer.Repositories.GraphR.GraphDetailR;
using Shared.Dto.Graph.Item;
using Shared.Dto.Graph.Read.Open;
using BizLayer.Builders.GraphBuilders;

namespace ServiceLayer.Graphs.Services;

public class OperationGraphDetailItemService : ErrorsMapper, IOperationGraphDetailItemService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraphDetailItem> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphDetailItemService(IMapper dataMapper, DbApplicationContext context)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление операции тех процесса в деталь графика. Добавлени происходит в самый конец
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns></returns>
    public async Task AddAsync(GraphDetailItemDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdWithItemsAsync(_context, dto.GraphDetailId, this);
        if (detail?.OperationGraphDetailItems!.Any(i => i.TechnologicalProcessItemId == dto.TechProcessItemId) ?? true)
            AddErrors("Деталь графика содержит данную операцию тех процесса");

        detail!.OperationGraphDetailItems!.Add(new OperationGraphDetailItem
        {
            TechnologicalProcessItemId = dto.TechProcessItemId,
            OrdinalNumber = detail.OperationGraphDetailItems.Max(i => i.OrdinalNumber) + 1
        });

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Добавление операции тех процесса в конец блока операций детали графика (где priority % 5 != 0)
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns></returns>
    public async Task AddToBlockAsync(AddToBlockGraphDetailItemDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdWithItemsAsync(_context, dto.GraphDetailId, this);
        
        OperationGraphDetailItemValidations.ValidationBeforeAddToBlock(detail, dto.TechProcessItemId, dto.Priority, this);
        if (HasErrors) return;

        foreach (var item in detail!.OperationGraphDetailItems!
                     .Where(i => i.OrdinalNumber > detail.OperationGraphDetailItems!
                         .Last(tpi => tpi.TechnologicalProcessItem!.Priority == dto.Priority).OrdinalNumber))
        {
            item.OrdinalNumber++;
        }

        detail.OperationGraphDetailItems!.Add(new OperationGraphDetailItem
        {
            TechnologicalProcessItemId = dto.TechProcessItemId,
            OrdinalNumber = detail.OperationGraphDetailItems.Last(i => i.TechnologicalProcessItem!.Priority == dto.Priority).OrdinalNumber + 1
        });

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Смена ветки для операции или блока операций детали графика
    /// </summary>
    /// <param name="dto">Информация для смены ветки</param>
    /// <returns></returns>
    public async Task SubstitutionToBranchAsync(SubstitutionToBranchDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdWithItemsAsync(_context, dto.GraphDetailId, this);
        if (detail?.OperationGraphDetailItems!.All(i => i.TechnologicalProcessItem!.Priority != dto.OldDetailItemPriority) ?? true)
            AddErrors($"В операций не найден переданный приориетет {dto.OldDetailItemPriority}");
        
        var newDetailItems = await TechProcessItemSimpleRead.GetBranchItemsAsync(_context, detail?.TechnologicalProcessId!.Value ?? 0, dto.NewDetailItemPriority, _dataMapper);
        if (newDetailItems.IsNullOrEmpty())
            AddErrors($"Не удалось получить операции тех процесса с приоритетом {detail?.TechnologicalProcessId}");

        if (HasErrors) return;

        var startNumber = detail!.OperationGraphDetailItems!.First(i => i.TechnologicalProcessItem!.Priority == dto.OldDetailItemPriority).OrdinalNumber;
        
        detail.OperationGraphDetailItems!.RemoveAll(i => i.TechnologicalProcessItem!.Priority == dto.OldDetailItemPriority);

        for (var i = 0; i < detail.OperationGraphDetailItems.Count(i => i.OrdinalNumber > startNumber); i++)
        {
            detail.OperationGraphDetailItems[i].OrdinalNumber = startNumber + newDetailItems.Count + i;
        }

        for (var i = 0; i < newDetailItems.Count; i++)
        {
            detail.OperationGraphDetailItems.Add(new OperationGraphDetailItem
            {
                TechnologicalProcessItemId = newDetailItems[i].TechProcessItemId,
                OrdinalNumber = startNumber + i
            });
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Свап блоков операций детали графика
    /// </summary>
    /// <param name="dto">Информация для свапа блоков</param>
    /// <returns></returns>
    public async Task SwapBlocksAsync(SwapGraphDetailItemBlocksDto dto)
    {
        if (dto.SourceItemPriority == dto.TargetItemPriority)
            AddErrors("Нельзя выбирать один и тот же блок");

        var targetItems = await OperationGraphDetailItemRead.ItemsByPriorityAsync(_repository, dto.GraphDetailId, dto.TargetItemPriority, this);
        var sourceItems = await OperationGraphDetailItemRead.ItemsByPriorityAsync(_repository, dto.GraphDetailId, dto.SourceItemPriority, this);

        if (HasErrors) return;

        int[] list =
        {
            (targetItems!.First().OrdinalNumber < sourceItems!.First().OrdinalNumber ? targetItems!.Last().OrdinalNumber : sourceItems!.Last().OrdinalNumber),
            (targetItems!.First().OrdinalNumber < sourceItems!.First().OrdinalNumber ? sourceItems!.First().OrdinalNumber : targetItems!.First().OrdinalNumber)
        };
        var middleItems = await OperationGraphDetailItemRead.ItemByRangeNumbersAsync(_repository, dto.GraphDetailId, list.Min(), list.Max(), this);

        var startNumber = list.Min() == targetItems!.Last().OrdinalNumber
            ? targetItems!.First().OrdinalNumber
            : sourceItems!.First().OrdinalNumber;

        var orderingList = new List<OperationGraphDetailItem>();

        orderingList.AddRange(dto.TargetItemPriority > dto.SourceItemPriority ? targetItems! : sourceItems!);
        orderingList.AddRange(middleItems);
        orderingList.AddRange(dto.TargetItemPriority > dto.SourceItemPriority ? sourceItems! : targetItems!);

        foreach (var item in orderingList)
        {
            item.OrdinalNumber = startNumber;
            startNumber++;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Свап операций детали графика внутри блока операций
    /// </summary>
    /// <param name="dto">Информация для свапа</param>
    /// <returns></returns>
    public async Task SwapInBlockAsync(SwapGraphDetailItemsInBlockDto dto)
    {
         var targetItem = await OperationGraphDetailItemRead.ByIdAsync(_repository, dto.TargetDetailItemId, this);
         var sourceItem = await OperationGraphDetailItemRead.ByIdAsync(_repository, dto.SourceDetailItemId, this);

         if (targetItem?.OperationGraphDetailId != sourceItem?.OperationGraphDetailId)
             AddErrors("Операции принадлежат разным деталям графика");
         
         if (HasErrors) return;

         (targetItem!.OrdinalNumber, sourceItem!.OrdinalNumber) = (sourceItem.OrdinalNumber, targetItem.OrdinalNumber);

         await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка блока операций детали графика между двумя другими блоками
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenBlocksAsync(GraphDetailItemInsertBetweenBlocksDto dto)
    { 
        var targetItems = await OperationGraphDetailItemRead.ItemsByPriorityAsync(_repository, dto.GraphDetailId, dto.TargetItemPriority, this);
        if (HasErrors) return;

        var isNewNumerSmallerOld = targetItems!.First().OrdinalNumber > dto.NewFirstItemNumber;
            
        var startNumber = isNewNumerSmallerOld
            ? dto.NewFirstItemNumber - 1
            : targetItems!.Last().OrdinalNumber;

        var endNumber = isNewNumerSmallerOld
            ? targetItems!.First().OrdinalNumber 
            : dto.NewFirstItemNumber + 1;

        var changingItem = await OperationGraphDetailItemRead.ItemByRangeNumbersAsync(_repository, dto.GraphDetailId, startNumber, endNumber, this);
        if (HasErrors) return;

        changingItem!.ForEach(i => i.OrdinalNumber += isNewNumerSmallerOld ? targetItems!.Count : targetItems!.Count * -1);

        for (var i = 0; i < targetItems!.Count; i++)
        {
            targetItems[i].OrdinalNumber = isNewNumerSmallerOld 
                ? dto.NewFirstItemNumber + i
                : changingItem.Last().OrdinalNumber + 1 + i;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка операции детали графика между двумя другими в пределах одного блока
    /// </summary>
    /// <param name="dto">Информация для вставки между</param>
    /// <returns></returns>
    public async Task InsertBetweenItemsInBlockAsync(InsertBetweenGraphDetailItemInBlockDto dto)
    {
        var targetItem = await OperationGraphDetailItemRead.ByNumberAsync(_repository, dto.GraphDetailId, dto.TargetItemNumber, this);
        await OperationGraphDetailItemRead.LoadTechProcessItem(_repository, targetItem);
        if (HasErrors) return;
        
        // TODO: Переписать как вставка в деталях графика

        var isNewNumerSmallerOld = targetItem!.OrdinalNumber > dto.NewItemNumber;

        var startNumber = isNewNumerSmallerOld
            ? dto.NewItemNumber - 1
            : targetItem.OrdinalNumber;

        var endNumber = isNewNumerSmallerOld
            ? targetItem.OrdinalNumber
            : dto.NewItemNumber + 1;

        var changingItem = await OperationGraphDetailItemRead.ItemByRangeNumbersAsync(_repository, dto.GraphDetailId, startNumber, endNumber, this);
        await OperationGraphDetailItemRead.LoadTechProcessItemForRangeItems(_repository, changingItem);

        OperationGraphDetailItemValidations.ValidationItemsPriorityEquals(changingItem, targetItem.TechnologicalProcessItem!.Priority, this);
        if (HasErrors) return;

        changingItem!.ForEach(i => i.OrdinalNumber += isNewNumerSmallerOld ? 1 : -1);

        targetItem.OrdinalNumber = isNewNumerSmallerOld 
            ? dto.NewItemNumber 
            : changingItem.Last().OrdinalNumber + 1;
       
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Временная конечная точка, добавление фактического количества деталей, которые прошли операцию детали графика
    /// </summary>
    /// <param name="dto">Информация для добавления фактического количества</param>
    /// <returns></returns>
    public async Task AddFactCountAsync(AddFactCountDto dto)
    {
        var item = await OperationGraphDetailItemRead.ByIdAsync(_repository, dto.GraphDetailItemId, this);
        if (HasErrors) return;

        if (item!.FactCount == dto.FactCount) return;
        
        item.FactCount = dto.FactCount;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление операции детали графика
    /// </summary>
    /// <param name="detailItemId">Id операции детали графика</param>
    /// <returns></returns>
    public async Task DeleteAsync(int detailItemId)
    {
        var item = await OperationGraphDetailItemRead.ByIdAsync(_repository, detailItemId, this);
        if (HasErrors) return;

        var itemsWithHigherNumber = await OperationGraphDetailItemRead.ItemsWithHigherNumberAsync(_repository, item!.OperationGraphDetailId, item.OrdinalNumber);
        itemsWithHigherNumber?.ForEach(i => i.OrdinalNumber--);

        await _context.RemoveWithValidationAndSaveAsync(item, this);
    }

    /// <summary>
    /// Получение операции детали графика
    /// </summary>
    /// <param name="graphDetailItemId">Id операции</param>
    /// <returns>Операция детали графика</returns>
    public async Task<InterimGraphDetailItemDto?> ByIdAsync(int graphDetailItemId) =>
        await OperationGraphDetailItemRead.ItemForOpenAsync(_context, graphDetailItemId, _dataMapper, this);

    /// <summary>
    /// Получение списка операций детали графика
    /// </summary>
    /// <param name="dto">Информация для получения списка операций</param>
    /// <returns>Список операций</returns>
    public async Task<GraphDetailItemHigherDto?> AllAsync(GetAllDetailItemsDto dto)
    {
        var dict = new Dictionary<int, int> { { dto.GraphDetailId, dto.TechProcessId } };
        var builder = new OpenOperationGraphItemBuilder(dict, _context, _dataMapper, this);

        var result = await builder.BuildAsync();
        return result[dto.GraphDetailId];
    }

    /// <summary>
    /// Получаем список операций тех процесса, которых еще нет в списке операций деталий графика, так же учитывается, что некоторые операции могут быть заменены на ответвления
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций тех процесса, которые можно добавить</returns>
    public async Task<List<GetAllToAddToEndDto>?> AllToAddToEndOfMainAsync(AddToEndOfMainInfoDto dto)
    {
        var techProcessItems = await TechProcessItemSimpleRead.GetItemIdsWithPrioritiesAsync(_context, dto.TechProcessId);
        var detailItemsPriorities = await OperationGraphDetailItemRead.ItemsPrioritiesAsync(_repository, dto.GraphDetailId);

        if (detailItemsPriorities is null || detailItemsPriorities.Count > techProcessItems.Count)
        {
            AddErrors("Не валидное количество операций детали графика");
            return null;
        }
        if (detailItemsPriorities.Count == techProcessItems.Count) return null;

        var disabledItems = new List<int>();

        foreach (var item in techProcessItems)
        {
            if (!detailItemsPriorities.Any(p => p == item.Value || (item.Value < p && p < item.Value + 5)))
                disabledItems.Add(item.Key);
        }

        var result = await TechProcessItemSimpleRead.GetItemsByRangeIdsAsync(_context, disabledItems, _dataMapper);

        return result;
    }

    /// <summary>
    /// Получение списка операций ветки тех процесса, которые можно добавить в блок операций детали графика
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций тех процесса, которые можно добавить</returns>
    public async Task<List<GetAllToAddToEndDto>?> AllToAddToEndOfBranchAsync(GetAllToAddToEndOfBranchDto dto)
    {
        var techProcessItems = await TechProcessItemSimpleRead.GetBranchItemsAsync(_context, dto.TechProcessId, dto.Priority, _dataMapper);
        var detailItemsOperationNumbers = await OperationGraphDetailItemRead.BranchItemsOperationNumbersAsync(_repository, dto.GraphDetailId, dto.Priority);

        if (detailItemsOperationNumbers is null || detailItemsOperationNumbers.Count > techProcessItems.Count)
        {
            AddErrors("Не валидное количество операций детали графика");
            return null;
        }
        if (detailItemsOperationNumbers.Count == techProcessItems.Count) return null;

        var disabledItems = new List<int>();

        foreach (var item in techProcessItems)
        {
            if (detailItemsOperationNumbers.All(p => p != item.OperationNumber))
                disabledItems.Add(item.TechProcessItemId);
        }

        var result = await TechProcessItemSimpleRead.GetItemsByRangeIdsAsync(_context, disabledItems, _dataMapper);

        return result;
    }

    /// <summary>
    /// Получение списка операций ответвления, с учетом того, что передаваться может приоритет ветки (main включительно)
    /// </summary>
    /// <param name="dto">Информация для выборки</param>
    /// <returns>Список операций ответвления</returns>
    public async Task<List<GetAllBranchesItemsDto>?> AllBranchesItemsAsync(BranchesItemsInfoDto dto)
    {
        var branchesItems = await TechProcessItemSimpleRead.GetBranhesItemsAsync(_context, dto.TechProcessId, dto.Priority, _dataMapper);
        if (branchesItems.IsNullOrEmpty())
        {
            AddErrors("Не удалось получить ответвления");
            return null;
        }

        var result = new List<GetAllBranchesItemsDto>();

        foreach (var item in branchesItems)
        {
            if (result.All(obj => obj.Priority != item.Priority))
                result.Add(new GetAllBranchesItemsDto
                {
                    Priority = item.Priority,
                    IsMainBranch = item.Priority % 5 == 0,
                    Items = new List<GetAllToAddToEndDto>()
                });

            result.Single(obj => obj.Priority == item.Priority).Items.Add(_dataMapper.Map<GetAllToAddToEndDto>(item));
        }

        return result;
    }

    public void Dispose() => _context.Dispose();
}