using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;
using Shared.Enums;

namespace BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;

public class TechProcessItemHardRead
{
    private readonly BaseModelRequests<TechnologicalProcessItem> _repository;
    private readonly ErrorsMapper _mapper;

    public TechProcessItemHardRead(BaseModelRequests<TechnologicalProcessItem> repository, ErrorsMapper mapper) => (_mapper, _repository) = (mapper, repository);

    public async Task<List<TechnologicalProcessItem>?> GetMainItemsWithRangeForNumberAsync(int firstNumber, int lastNumber, int techProcessId)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId
                           && tpi.MainTechnologicalProcessItemId == null
                           && (firstNumber > lastNumber ? (lastNumber <= tpi.Number && tpi.Number < firstNumber)
                               : (lastNumber >= tpi.Number && tpi.Number > firstNumber)),
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff));
        if (items is null)
            _mapper.AddErrors("Не удалось получить операции");
        return items;
    }

    public async Task<List<TechnologicalProcessItem>?> GetBranchItemsWithRangeForNumberAsync(int firstNumber, int lastNumber, int techProcessId, int mainItemId, int priority)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId
                           && tpi.MainTechnologicalProcessItemId == mainItemId
                           && tpi.Priority == priority
                           && (firstNumber > lastNumber ? (lastNumber <= tpi.Number && tpi.Number < firstNumber)
                               : (lastNumber >= tpi.Number && tpi.Number > firstNumber)),
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff));
        if (items is null)
            _mapper.AddErrors("Не удалось получить операции");
        return items;
    }

    public async Task<List<TechnologicalProcessItem>?> GetBranchItemsAsync(int mainItemId, int branchNumber)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.MainTechnologicalProcessItemId == mainItemId && tpi.Priority == branchNumber,
            trackingOptions: TrackingOptions.WithTracking);
        if (items.IsNullOrEmpty())
            _mapper.AddErrors($"Количество операций существующий ветки {branchNumber} операции тех процесса равно 0");
        return items;
    }

    public async Task<List<TechnologicalProcessItem>?> GetBranchItemsAsync(int mainItemId, int branchNumber, QueryFilterOptions options)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.MainTechnologicalProcessItemId == mainItemId && tpi.Priority == branchNumber,
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)options));
        if (items.IsNullOrEmpty())
            _mapper.AddErrors($"Количество операций существующий ветки {branchNumber} операции тех процесса равно 0");
        return items;
    }

    public async Task<List<TechnologicalProcessItem>?> GetBranchesItemsWithoutValidationAsync(int techProcessId, int mainItemId, QueryFilterOptions options) =>
        await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId
                           && tpi.MainTechnologicalProcessItemId == mainItemId,
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)options));

    public async Task<List<TechnologicalProcessItem>?> GetBranchesItemsAsync(int mainTechProcessItemId, List<int> branches, QueryFilterOptions options)
    {
        var items = await _repository.GetAllAsync(
            filter: tpi => tpi.MainTechnologicalProcessItemId == mainTechProcessItemId && branches.Contains(tpi.Priority),
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)options));
        if (items is null)
            _mapper.AddErrors("Не удалось получить список операций ветки");
        return items;
    }

    public List<int> GetBranchNumbersWithRequirement(List<int> fullBranchNumbers, Func<int, bool> requirement)
    {
        if (fullBranchNumbers.IsNullOrEmpty())
            _mapper.AddErrors("Не удалось получить список веток операции");
        return fullBranchNumbers.Where(requirement).ToList();
    }

    public async Task<List<TechnologicalProcessItem>?> GetMainWithIncludeBranchesItems(int techProcessId, List<int> techProcessItemsId)
    {
        if (techProcessId == 0)
            return null;

        var newBranchItems = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId
                           && tpi.MainTechnologicalProcessItemId == null
                           && techProcessItemsId.Contains(tpi.Id),
            include: i => i.Include(tpi => tpi.BranchesTechnologicalProcessItems!),
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            trackingOptions: TrackingOptions.WithTracking);

        if (newBranchItems is null || newBranchItems.Count != techProcessItemsId.Count)
            _mapper.AddErrors("Не удалось получить все операции для вставки");
            
        foreach (var item in newBranchItems!.Where(item => item.BranchesTechnologicalProcessItems!.Count > 0))
            _mapper.AddErrors($"В операции {item.OperationNumber} есть ответвления");

        return newBranchItems;
    }

    public async Task<List<TechnologicalProcessItem>?> GetMainWithIncludeBranchesItems(int techProcessId, List<int> techProcessItemsId, QueryFilterOptions options)
    {
        var newBranchItems = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == techProcessId
                           && tpi.MainTechnologicalProcessItemId == null
                           && techProcessItemsId.Contains(tpi.Id),
            include: i => i.Include(tpi => tpi.BranchesTechnologicalProcessItems!),
            orderBy: o => o.OrderBy(tpi => tpi.Number),
            addQueryFilters: Convert.ToBoolean((int)options),
            trackingOptions: TrackingOptions.WithTracking);

        if (newBranchItems is null || newBranchItems.Count != techProcessItemsId.Count)
            _mapper.AddErrors("Не удалось получить все операции для вставки");

        foreach (var item in newBranchItems!.Where(item => item.BranchesTechnologicalProcessItems!.Count > 0))
            _mapper.AddErrors($"В операции {item.OperationNumber} есть ответвления");

        return newBranchItems;
    }
}