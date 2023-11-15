using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace DatabaseLayer.Options.GraphO;

public static class OperationGraphDetailOptions
{
    public static IQueryable<OperationGraphDetail> Visibility(this IQueryable<OperationGraphDetail> details, GraphDetailVisibility detailVisibility)
    {
        return detailVisibility switch
        {
            GraphDetailVisibility.All => details.IgnoreQueryFilters(),
            GraphDetailVisibility.InWork => details,
            _ => details
        };
    }

    public static IQueryable<OperationGraphDetail> GraphOpen(this IQueryable<OperationGraphDetail> details, GraphOpenType openType)
    {
        return openType switch
        {
            GraphOpenType.WithRepeats => details.OrderBy(d => d.DetailGraphNumber),
            GraphOpenType.WithoutRepeats => details.Where(d => d.DetailGraphNumberWithoutRepeats > 0).OrderBy(d => d.DetailGraphNumberWithoutRepeats),
            _ => throw new ArgumentException("Не валидный тип открытия графика", nameof(openType))
        };
    }
}