using DB.Model.StorageInfo.Graph;
using Shared.BasicStructuresExtensions;
using Shared.Enums;

namespace DatabaseLayer.Options.GraphO;

public static class OperationGraphOptions
{
    public static IQueryable<OperationGraph> FromDate(this IQueryable<OperationGraph> graphs, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is null || endDate is null)
            return graphs;

        return graphs.Where(g =>
            (g.GraphDate.Month >= startDate.Value.Month && g.GraphDate.Year >= startDate.Value.Year) 
            && (g.GraphDate.Month <= endDate.Value.Month && g.GraphDate.Year <= endDate.Value.Year));
    }

    public static IQueryable<OperationGraph> FromSubdivisionId(this IQueryable<OperationGraph> graphs, int subdivisionId)
    {
        return subdivisionId == 0
            ? graphs
            : graphs.Where(g => g.SubdivisionId == subdivisionId);
    }

    public static IQueryable<OperationGraph> FromStatusId(this IQueryable<OperationGraph> graphs, int statusId)
    {
        return statusId == 0
            ? graphs
            : graphs.Where(g => g.StatusId == statusId);
    }

    public static IQueryable<OperationGraph> FromOwnershipType(this IQueryable<OperationGraph> graphs, GraphOwnershipType graphOwnershipType, int userId)
    {
        return graphOwnershipType switch
        {
            GraphOwnershipType.Owner => graphs.Where(g => g.OwnerId == userId),
            GraphOwnershipType.AccessProvided => graphs.Where(g => g.OwnerId != userId && g.OperationGraphUsers!.Any(gu => gu.UserId == userId)),
            _ => graphs.Where(g => g.OwnerId == userId || g.OperationGraphUsers!.Any(gu => gu.UserId == userId))
        };
    }

    public static IQueryable<OperationGraph> FromAccess(this IQueryable<OperationGraph> graphs, int userId, GraphAccessTypeForFilters accessType)
    {
        return accessType switch
        {
            GraphAccessTypeForFilters.ReadAndEdit => graphs.Where(g =>
                g.OwnerId == userId || g.OperationGraphUsers!.Any(gu => gu.UserId == userId && !gu.IsReadonly)),
            GraphAccessTypeForFilters.Readonly => graphs.Where(g =>
                g.OperationGraphUsers!.Any(gu => gu.UserId == userId && gu.IsReadonly)),
            _ => graphs
        };
    }

    public static IQueryable<OperationGraph> FromProductAavailability(this IQueryable<OperationGraph> graphs, GraphProductAvailability availability)
    {
        return availability switch
        {
            GraphProductAvailability.Have => graphs.Where(g => g.ProductDetailId.HasValue),
            GraphProductAvailability.HaveNot => graphs.Where(g => !g.ProductDetailId.HasValue),
            _ => graphs
        };
    }

    public static IQueryable<OperationGraph> FromIgnoredGraphIds(this IQueryable<OperationGraph> graphs, List<int>? ignoredGraphIds)
    {
        return ignoredGraphIds.IsNullOrEmpty()
            ? graphs
            : graphs.Where(g => !ignoredGraphIds!.Contains(g.Id));
    }
}