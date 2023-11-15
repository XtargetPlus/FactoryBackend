using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;
using Shared.Enums;

namespace BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;

public class TechProcessItemRepository
{
    private readonly BaseModelRequests<TechnologicalProcessItem> _repository;
    private readonly ErrorsMapper _mapper;

    public TechProcessItemRepository(BaseModelRequests<TechnologicalProcessItem> repository, ErrorsMapper mapper) => (_mapper, _repository) = (mapper, repository);

    public IEnumerable<TechnologicalProcessItem> ChangeNumberAndPriority(IEnumerable<TechnologicalProcessItem> items, bool isIncrement, bool isMainBranch)
    {
        foreach (var item in items)
        {
            item.Number += isIncrement ? 1 : -1;
            item.Priority += isMainBranch ? (isIncrement ? 5 : -5) : 0;
        }
        return items;
    }

    /// <summary>
    /// Меняем приоритеты всем ветками и им операциями
    /// </summary>
    /// <param name="firstPriority">Приоритет первой операции (приоритет операции, к которой принадлежат ветки)</param>
    /// <param name="secondPriority">Приоритет второй операции (приоритет операции, на которую меняем)</param>
    /// <param name="branchNumbers">Список веток первой операции</param>
    /// <returns>Словарь, где Key - номер ветки, Value - операции данной ветки или null (ошибки)</returns>
    public async Task ChangeBranchesOperationsPriorityAsync(
        List<int> branchNumbers,
        int mainItemId,
        int firstPriority,
        int secondPriority)
    {
        TechProcessItemHardRead hardRead = new(_repository, _mapper);
        foreach (var branchNumber in branchNumbers)
        {
            var items = await hardRead.GetBranchItemsAsync(mainItemId, branchNumber, QueryFilterOptions.TurnOff);
            if (items is null)
                return;
            // меняем приоритеты местами:
            // 6 = 5 + 11 % 10
            // 7 = 5 + 12 % 10
            // 11 = 10 + 6 % 5
            // 12 = 10 + 7 % 5
            var priority = firstPriority + branchNumber % secondPriority;
            items.ForEach(item => item.Priority = priority);
        }
    }

    public async Task NumberAndPriorityRecalculationAsync(int techProcessId)
    {
        var mainItems = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId && tpi.MainTechnologicalProcessItemId == null,
            include: i => i.Include(tpi => tpi.BranchesTechnologicalProcessItems!),
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff),
            trackingOptions: TrackingOptions.WithTracking);
        if (mainItems.IsNullOrEmpty())
        {
            _mapper.AddErrors("Не удалось получить список операций тех процесса");
            return;
        }

        for (var i = 0; i < mainItems?.Count; i++)
        {
            mainItems[i].Number = i + 1;
            mainItems[i].Priority = 5 * (i + 1);
            mainItems[i].BranchesTechnologicalProcessItems!.ForEach(item => item.Priority = mainItems[i].Priority + item.Priority % 5);
        }
    }

    public async Task<int> RecalculationWithHigherPriorityAsync(int techProcessId, int priority)
    {
        var mainItems = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId && tpi.MainTechnologicalProcessItemId == null && tpi.Priority > priority,
            include: i => i.Include(tpi => tpi.BranchesTechnologicalProcessItems!),
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff),
            trackingOptions: TrackingOptions.WithTracking);

        if (mainItems is null)
        {
            _mapper.AddErrors("Не удалось получить список операций тех процесса");
            return 0;
        }

        if (mainItems.Count == 0)
            return 0;

        foreach (var item in mainItems)
        {
            item.Number--;
            item.Priority -= 5;
            item.BranchesTechnologicalProcessItems!.ForEach(branchItem => branchItem.Priority -= 5);
        }

        return 1;
    }

    public async Task RecalculateBranchItemsAsync(int mainItemId, int techProcessId, int priority, ErrorsMapper errors)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.Id != techProcessId && tpi.MainTechnologicalProcessItemId == mainItemId && tpi.Priority == priority,
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff),
            trackingOptions: TrackingOptions.WithTracking);

        if (items is null)
        {
            errors.AddErrors($"Не удалось получить операции ветки {priority}");
            return;
        }
        
        for (var i = 0; i < items.Count; i++)
            items[i].Number = i + 1;
    }
}