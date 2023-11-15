using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;
using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR.GraphDetailItemR;

public static class OperationGraphDetailItemRead
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="detailItemId"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetailItem?> ByIdAsync(BaseModelRequests<OperationGraphDetailItem> repository, int detailItemId, ErrorsMapper mapper)
    {
        var item = await repository.FindByIdAsync(detailItemId);
        
        if (item is null) mapper.AddErrors("Не удалось получить операцию детали графика");

        return item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphDetailId"></param>
    /// <param name="errorsMapper"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task<GraphDetailDto?> ByIdToGraphAsync(DbContext context, int graphDetailId, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        var detail = await context.Set<OperationGraphDetail>()
            .IgnoreQueryFilters()
            .Where(d => d.Id == graphDetailId)
            .ProjectTo<GraphDetailDto>(dataMapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (detail is null) errorsMapper.AddErrors("Не удалось получить деталь графика");

        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="detailItemId"></param>
    /// <param name="number"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetailItem?> ByNumberAsync(BaseModelRequests<OperationGraphDetailItem> repository, int detailItemId, int number, ErrorsMapper mapper)
    {
        var item = await repository.FindFirstAsync(filter: i => i.OperationGraphDetailId == detailItemId && i.OrdinalNumber == number);

        if (item is null) mapper.AddErrors("Не удалось получить операцию детали графика");

        return item;
    } 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="detailId"></param>
    /// <param name="priority"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetailItem>?> ItemsByPriorityAsync(
        BaseModelRequests<OperationGraphDetailItem> repository,
        int detailId,
        int priority,
        ErrorsMapper mapper)
    {
        var items = await repository.GetAllAsync(
            filter: i => i.OperationGraphDetailId == detailId && i.TechnologicalProcessItem!.Priority == priority,
            orderBy: o => o.OrderBy(i => i.OrdinalNumber),
            trackingOptions: TrackingOptions.WithTracking);

        if (items.IsNullOrEmpty()) mapper.AddErrors("Не удалось получить операции детали графика");

        return items;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="detailId"></param>
    /// <param name="startNumber"></param>
    /// <param name="endNumber"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetailItem>?> ItemByRangeNumbersAsync(
        BaseModelRequests<OperationGraphDetailItem> repository,
        int detailId,
        int startNumber,
        int endNumber,
        ErrorsMapper mapper)
    {
        var items = await repository.GetAllAsync(
            filter: i => i.OperationGraphDetailId == detailId 
                         && startNumber < i.OrdinalNumber
                         && i.OrdinalNumber < endNumber,
            orderBy: o => o.OrderBy(i => i.OrdinalNumber),
            trackingOptions: TrackingOptions.WithTracking);

        if (items.IsNullOrEmpty()) mapper.AddErrors("Не удалось получить операции детали графика");

        return items;
    } 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="operationDetailId"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetailItem>?> ItemsWithHigherNumberAsync(BaseModelRequests<OperationGraphDetailItem> repository, int operationDetailId, int number) =>
        await repository.GetAllAsync(
            filter: i => i.OperationGraphDetailId == operationDetailId && i.OrdinalNumber > number,
            trackingOptions: TrackingOptions.WithTracking);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="graphDetailId"></param>
    /// <returns></returns>
    public static async Task<List<int>?> ItemsPrioritiesAsync(BaseModelRequests<OperationGraphDetailItem> repository, int graphDetailId) =>
        await repository.GetAllAsync(
            filter: i => i.OperationGraphDetailId == graphDetailId,
            select: i => i.TechnologicalProcessItem!.Priority,
            distinct: true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="graphDetailId"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    public static async Task<List<string>?> BranchItemsOperationNumbersAsync(BaseModelRequests<OperationGraphDetailItem> repository, int graphDetailId, int priority) =>
        await repository.GetAllAsync(
            filter: i => i.OperationGraphDetailId == graphDetailId && i.TechnologicalProcessItem!.Priority == priority,
            select: i => i.TechnologicalProcessItem!.OperationNumber,
            distinct: true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphs"></param>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task LoadDetailsItemsAsync(List<OperationGraph> graphs, DbContext context, IMapper dataMapper)
    {
        var graphDetailRepository = new BaseModelRequests<OperationGraphDetail>(context, dataMapper);

        foreach (var graph in graphs)
        {
            for (var i = 0; i < graph.OperationGraphDetails!.Count; i++)
            {
                graph.OperationGraphDetails[i] = await graphDetailRepository
                    .IncludeCollectionAsync(graph.OperationGraphDetails[i], d => d.OperationGraphDetailItems);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    public static async Task LoadDetailItemsAsync(DbContext context, OperationGraphDetail? detail)
    {
        var repository = new BaseModelRequests<OperationGraphDetail>(context, null);
        await repository.IncludeCollectionAsync(detail, d => d.OperationGraphDetailItems!);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static async Task LoadTechProcessItemForRangeItems(BaseModelRequests<OperationGraphDetailItem> repository, List<OperationGraphDetailItem>? items)
    {
        if (items is null) return;

        foreach (var item in items)
        {
            await repository.InculdeReferenceAsync(item, i => i.TechnologicalProcessItem);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static async Task LoadTechProcessItem(BaseModelRequests<OperationGraphDetailItem> repository, OperationGraphDetailItem? item)
    {
        if (item is null) return;

        await repository.InculdeReferenceAsync(item, i => i.TechnologicalProcessItem);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="priority"></param>
    /// <param name="mainItemId"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<bool> BranchPossibleToAddToEndAsync(int count, int priority, int mainItemId, DbContext context) =>
        await context.Set<TechnologicalProcessItem>()
            .CountAsync(tpi => tpi.MainTechnologicalProcessItemId == mainItemId && tpi.Priority == priority) < count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphDetailItemId"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errorsMapper"></param>
    /// <returns></returns>
    public static async Task<InterimGraphDetailItemDto?> ItemForOpenAsync(DbContext context, int graphDetailItemId, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        var item = await context.Set<OperationGraphDetailItem>()
            .AsNoTracking()
            .Where(i => i.Id == graphDetailItemId)
            .ProjectTo<InterimGraphDetailItemDto>(dataMapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (item is null) errorsMapper.AddErrors("Не удалось получить операцию детали графика");

        return item;
    }
}