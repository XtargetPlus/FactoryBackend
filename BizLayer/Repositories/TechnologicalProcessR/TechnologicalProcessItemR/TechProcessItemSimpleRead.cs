using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.BranchesItems;
using Shared.Enums;

namespace BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;

public static class TechProcessItemSimpleRead
{
    public static async Task<TechnologicalProcessItem?> GetAsync(BaseModelRequests<TechnologicalProcessItem> repository, int techProcessItemId, ErrorsMapper mapper)
    {
        var techProcessItem = await repository.FindByIdAsync(techProcessItemId);
        if (techProcessItem is null)
            mapper.AddErrors("Не удалось получить операцию тех процесса");
        return techProcessItem;
    }

    public static async Task<BranchItemDto?> GetAsync(DbContext context, int techProcessItemId, IMapper dataMapper, ErrorsMapper errors)
    {
        var techProcessItem = await context.Set<TechnologicalProcessItem>()
            .Where(tpi => tpi.Id == techProcessItemId)
            .ProjectTo<BranchItemDto>(dataMapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        if (techProcessItem is null)
            errors.AddErrors($"Не удалось получить операцию тех процесса {techProcessItem}");

        return techProcessItem;
    }

    public static async Task<TechnologicalProcessItem?> GetAsync(BaseModelRequests<TechnologicalProcessItem> repository, int techProcessItemId, QueryFilterOptions options, ErrorsMapper mapper)
    {
        var techProcessItem = await repository.FindFirstAsync(
            filter: tpi => tpi.Id == techProcessItemId,
            addQueryFilters: Convert.ToBoolean((int)options));
        if (techProcessItem is null)
            mapper.AddErrors("Не удалось получить операцию тех процесса");
        return techProcessItem;
    }

    public static async Task<Dictionary<int, List<int>>> GetItemsByTechProcessIdListAsync(DbContext context, List<int> techProcessesId, ErrorsMapper mapper)
    {
        if (techProcessesId.Count == 0)
            mapper.AddErrors("Невозможно получить операции тех процессов, список тех процессов пуст");

        var techProcessItemRepository = context.Set<TechnologicalProcessItem>();

        var techProcessAndItems = await techProcessItemRepository
            .Where(tpi => techProcessesId.Contains(tpi.TechnologicalProcessId) && tpi.MainTechnologicalProcessItemId == null)
            .OrderBy(tpi => tpi.Number)
            .Select(tpi => new { TechProcessId = tpi.TechnologicalProcessId, ItemId = tpi.Id })
            .ToListAsync();

        var result = techProcessesId.ToDictionary(
            techProcessId => techProcessId,
            techProcessId => techProcessAndItems.Where(t => t.TechProcessId == techProcessId).Select(t => t.ItemId).ToList()
        );

        return result;
    }

    public static async Task<TechnologicalProcessItem?> GetWithTechProcessIdAsync(BaseModelRequests<TechnologicalProcessItem> repository, int techProcessId, int techProcessItemId, ErrorsMapper mapper)
    {
        var item = await repository.FindFirstAsync(filter: tpi => tpi.Id == techProcessItemId && tpi.TechnologicalProcessId == techProcessId);
        if (item is null)
            mapper.AddErrors("Не удалось получить операцию тех процесса");
        return item;
    }

    public static async Task<TechnologicalProcessItem?> GetWithIncludeOperationAsync(
        BaseModelRequests<TechnologicalProcessItem> repository, 
        int techProcessItemId, 
        QueryFilterOptions options,
        ErrorsMapper mapper)
    {
        var item = await repository.FindFirstAsync(
            filter: tpi => tpi.Id == techProcessItemId,
            include: i => i.Include(tpi => tpi.Operation!),
            addQueryFilters: Convert.ToBoolean((int)options));
        if (item is null)
            mapper.AddErrors("Не удалось получить операцию тех процесса");
        return item;
    }

    public static async Task<bool> IsHaveBranchesAsync(DbContext context, int techProcessItemId) =>
        await context.Set<TechnologicalProcessItem>()
            .CountAsync(tpi => tpi.MainTechnologicalProcessItemId.HasValue && tpi.MainTechnologicalProcessItemId.Value == techProcessItemId) > 0;

    public static async Task<int?> GetMainIdAsync(DbContext context, int branchItemId) =>
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => tpi.Id == branchItemId)
            .Select(tpi => tpi.MainTechnologicalProcessItemId)
            .FirstAsync();

    public static async Task<int> GetMainsCountAsync(DbContext context, int techProcessId) =>
        await context.Set<TechnologicalProcessItem>()
            .CountAsync(tpi => tpi.TechnologicalProcessId == techProcessId && !tpi.MainTechnologicalProcessItemId.HasValue);

    public static async Task<List<int>> GetMainIdsAsync(DbContext context, int detailId, int techProcessId) => 
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => 
                tpi.TechnologicalProcessId == techProcessId && tpi.TechnologicalProcess!.DetailId == detailId && !tpi.MainTechnologicalProcessItemId.HasValue)
            .OrderBy(tpi => tpi.Number)
            .Select(tpi => tpi.Id)
            .ToListAsync();

    public static async Task<Dictionary<int, int>> GetItemIdsWithPrioritiesAsync(DbContext context, int techProcessId) =>
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => tpi.TechnologicalProcessId == techProcessId && !tpi.MainTechnologicalProcessItemId.HasValue)
            .OrderBy(tpi => tpi.Number)
            .Select(tpi => new { Key = tpi.Id, Value = tpi.Priority })
            .ToDictionaryAsync(k => k.Key, v => v.Value);

    public static async Task<List<GetAllToAddToEndDto>> GetItemsByRangeIdsAsync(DbContext context, List<int> techProcessIds, IMapper dataMapper) =>
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => techProcessIds.Contains(tpi.Id))
            .OrderBy(tpi => tpi.Number)
            .ProjectTo<GetAllToAddToEndDto>(dataMapper.ConfigurationProvider)
            .ToListAsync();

    public static async Task<List<BranchItemDto>> GetBranhesItemsAsync(DbContext context, int techProcessId, int priority, IMapper dataMapper) =>
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => 
                tpi.TechnologicalProcessId == techProcessId 
                && tpi.Priority != priority 
                && (tpi.Priority == priority - priority % 5 || (priority - priority % 5 < tpi.Priority && tpi.Priority < (priority - priority % 5) + 5)))
            .OrderBy(tpi => tpi.Priority)
            .ProjectTo<BranchItemDto>(dataMapper.ConfigurationProvider)
            .ToListAsync();

    public static async Task<List<BranchItemDto>> GetBranchItemsAsync(DbContext context, int techProcessId, int priority, IMapper dataMapper) =>
        await context.Set<TechnologicalProcessItem>()
            .Where(tpi => tpi.TechnologicalProcessId == techProcessId && tpi.Priority == priority)
            .OrderBy(tpi => tpi.Number)
            .ProjectTo<BranchItemDto>(dataMapper.ConfigurationProvider)
            .ToListAsync();
}