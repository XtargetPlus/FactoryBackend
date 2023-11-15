using DB.Model.TechnologicalProcessInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.OperationO;

public static class OperationOptions
{
    public static IQueryable<Operation> SearchOptions(this IQueryable<Operation> operations, string text = "", OperationFilterOptions searchOption = OperationFilterOptions.ForShortName)
    {
        if (string.IsNullOrEmpty(text))
            return operations;
        return searchOption switch
        {
            OperationFilterOptions.ForShortName => operations.Where(o => o.ShortName.Contains(text)),
            OperationFilterOptions.ForFullName => operations.Where(o => o.FullName.Contains(text)),
            _ => operations
        };
    }

    public static IQueryable<Operation> OrderOptions(this IQueryable<Operation> operations, OperationFilterOptions orderOption = default, KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOption switch
        {  
            OperationFilterOptions.ForShortName => kindOfOrder switch
            {
                KindOfOrder.Up => operations.OrderBy(o => o.ShortName),
                KindOfOrder.Down => operations.OrderByDescending(o => o.ShortName),
                _ => operations.OrderBy(o => o.Id)
            },
            OperationFilterOptions.ForFullName=> kindOfOrder switch
            {
                KindOfOrder.Up => operations.OrderBy(o => o.FullName),
                KindOfOrder.Down => operations.OrderByDescending(o => o.FullName),
                _ => operations.OrderBy(o => o.Id)
            },
            _ => operations.OrderBy(o => o.Id)
        };
    }
}