using AutoMapper;
using DatabaseLayer.DbRequests.GraphToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.DetailInfo;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR.GraphDetailR;

public class OperationGraphDetailRead
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="detailId"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetail?> ByIdAsync(BaseModelRequests<OperationGraphDetail> repository, int detailId, ErrorsMapper errors)
    {
        var detail = await repository.FindByIdAsync(detailId);
        if (detail is null) 
            errors.AddErrors("Не удалось получить деталь графика");

        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="graphId"></param>
    /// <param name="position"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetail?> ByPositionNumberToDisplayAsync(BaseModelRequests<OperationGraphDetail> repository, int graphId, int position, ErrorsMapper errors)
    {
        var detail = await repository.FindFirstAsync(filter: d => d.OperationGraphId == graphId && d.DetailGraphNumberWithoutRepeats == position);
        if (detail is null)
            errors.AddErrors("Не удалось получить деталь графика");

        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetail>?> DetailByPositionNumberToDisplayRangeAsync(DbContext context, int graphId, int min, int max, ErrorsMapper errors)
    {
        var details = await context.Set<OperationGraphDetail>()
            .Where(d =>
                d.OperationGraphId == graphId && d.DetailGraphNumberWithoutRepeats <= max &&
                d.DetailGraphNumberWithoutRepeats >= min)
            .OrderBy(d => d.DetailGraphNumberWithoutRepeats)
            .ToListAsync();
        
        if (details.IsNullOrEmpty())
            errors.AddErrors("Не удалось получить детали графика");

        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetail>?> DetailsWithNumberStartByAsync(DbContext context, OperationGraphDetail? detail)
    {
        if (detail is null) return null;

        return await context.Set<OperationGraphDetail>()
            .IgnoreQueryFilters()
            .Where(d => d.OperationGraphId == detail.OperationGraphId
                        && d.Id != detail.Id
                        && d.DetailGraphNumberWithRepeats.StartsWith(detail.DetailGraphNumberWithRepeats))
            .OrderBy(d => d.DetailGraphNumber)
            .ToListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <param name="detailNumber"></param>
    /// <returns></returns>
    public static async Task<float> MultipledFathersUsabilitiesByDetailNumber(DbContext context, int graphId, string detailNumber)
    {
        float result = 0;
        var detailSet = context.Set<OperationGraphDetail>();

        for (var i = detailNumber.Split('.')[..^1].Length; i > 0; i--)
        {
            var chars = detailNumber.Split('.')[..^i];

            var usability = await detailSet
                .Where(d => d.OperationGraphId == graphId && d.DetailGraphNumberWithRepeats == string.Join('.', chars))
                .Select(d => d.Usability)
                .FirstAsync();

            if (result == 0)
                result = usability;
            else
                result *= usability;
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detailId"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetail?> WithoutFiltersAsync(DbContext context, int detailId, ErrorsMapper errors)
    {
        var detail = await context.Set<OperationGraphDetail>().IgnoreQueryFilters().FirstOrDefaultAsync(d => d.Id == detailId);
        if (detail is null)
            errors.AddErrors("Не удалось получить деталь графика");

        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detailId"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetail?> ByIdWithItemsAsync(DbContext context, int detailId, ErrorsMapper errors)
    {
        var detail = await context.Set<OperationGraphDetail>()
            .Where(d => d.Id == detailId)
            .Include(d => d.OperationGraphDetailItems)
            !.ThenInclude(i => i.TechnologicalProcessItem)
            .FirstOrDefaultAsync();

        if (detail is null)
            errors.AddErrors("Не удалось получить деталь графика");

        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<List<GraphDetailDto>?> AllForOpenAsync(OpenOperationGraphFilters filters, DbContext context, IMapper dataMapper, ErrorsMapper errors)
    {
        var details = await new OperationGraphDetailRequests(context, dataMapper).GetDetailsForOpenAsync(filters);
        
        if (details is null)
            errors.AddErrors("Не удалось получить детали графика");

        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graph"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task<OperationGraph?> LoadDetailsAsync(DbContext context, OperationGraph? graph, IMapper dataMapper)
    {
        if (graph is null) return null;

        var graphRepository = new BaseModelRequests<OperationGraph>(context, dataMapper);

        await graphRepository.IncludeCollectionAsync(graph, g => g.OperationGraphDetails!, addQueryFilters: false);

        if (!(graph.OperationGraphMainGroups?.Any() ?? false)) return graph;

        foreach (var graphGroup in graph.OperationGraphMainGroups)
            await graphRepository.IncludeCollectionAsync(graphGroup.OperationGraphNext!, g => g.OperationGraphDetails!, addQueryFilters: false);

        return graph;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task LoadNextGroupAsync(DbContext context, List<OperationGraphDetail>? details, IMapper dataMapper)
    {
        if (details.IsNullOrEmpty()) return;

        var graphDetailRepository = new BaseModelRequests<OperationGraphDetail>(context, dataMapper);
        var graphDetailGroupRepository = new BaseModelRequests<OperationGraphDetailGroup>(context, dataMapper);

        foreach (var detail in details!.Where(d => !d.TotalPlannedNumber.HasValue))
        {
            await graphDetailRepository.IncludeCollectionAsync(detail, d => d.OperationGraphNextDetails!);

            foreach (var groupDetails in detail.OperationGraphNextDetails!)
                await graphDetailGroupRepository.InculdeReferenceAsync(groupDetails, g => g.OperationGraphMainDetail);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detail"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task LoadMainGroupDetail(DbContext context, OperationGraphDetail? detail, IMapper dataMapper)
    {
        if (detail is null) return;

        var graphDetailRepository = new BaseModelRequests<OperationGraphDetail>(context, dataMapper);
        var graphDetailGroupRepository = new BaseModelRequests<OperationGraphDetailGroup>(context, dataMapper);

        await graphDetailRepository.IncludeCollectionAsync(detail, d => d.OperationGraphMainDetails!);
        foreach (var nextDetalil in detail.OperationGraphMainDetails!)
        {
            await graphDetailGroupRepository.InculdeReferenceAsync(nextDetalil, d => d.OperationGraphNextDetail);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    /// <param name="dataMapper"></param>
    /// <returns></returns>
    public static async Task LoadMainGroupDetail(DbContext context, List<OperationGraphDetail>? details, IMapper dataMapper)
    {
        if (details.IsNullOrEmpty()) return;

        var graphDetailRepository = new BaseModelRequests<OperationGraphDetail>(context, dataMapper);
        var graphDetailGroupRepository = new BaseModelRequests<OperationGraphDetailGroup>(context, dataMapper);

        foreach (var detail in details!.Where(d => d.TotalPlannedNumber.HasValue))
        {
            await graphDetailRepository.IncludeCollectionAsync(detail, d => d.OperationGraphMainDetails);
            foreach (var nextDetail in detail.OperationGraphMainDetails!)
            {
                await graphDetailGroupRepository.InculdeReferenceAsync(nextDetail, d => d.OperationGraphNextDetail);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detailId"></param>
    /// <param name="ignoreGraphDetails"></param>
    /// <returns></returns>
    public static async Task<OperationGraphDetail?> NewMainAsync(DbContext context, int detailId, List<int> ignoreGraphDetails, List<int> graphGroupIds) =>
        await context.Set<OperationGraphDetail>()
            .Where(d => graphGroupIds.Contains(d.OperationGraphId) 
                        && d.DetailId == detailId 
                        && !ignoreGraphDetails.Contains(d.Id))
            .OrderBy(d => d.DetailGraphNumber)
            .FirstOrDefaultAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetail>> AllAsync(DbContext context, int graphId) =>
        await context.Set<OperationGraphDetail>()
            .Where(d => d.OperationGraphId == graphId)
            .OrderBy(d => d.DetailGraphNumber)
            .ToListAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphIds"></param>
    /// <returns></returns>
    public static async Task<List<OperationGraphDetail>> AllOrderByWithoutAsync(DbContext context, List<int> graphIds) =>
        await context.Set<OperationGraphDetail>()
            .Where(d => graphIds.Contains(d.OperationGraphId) && d.TotalPlannedNumber.HasValue)
            .OrderBy(d => d.DetailGraphNumberWithoutRepeats)
            .ToListAsync();
}