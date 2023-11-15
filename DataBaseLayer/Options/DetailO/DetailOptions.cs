using DB.Model.DetailInfo;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.BasicStructuresExtensions;

namespace DatabaseLayer.Options.DetailO;

public static class DetailOptions
{
    public static IQueryable<Detail> DetailSearch(this IQueryable<Detail> details, SerialNumberOrTitleFilter searchOptions, string text = "")
    {
        if (string.IsNullOrEmpty(text))
            return details;
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => details.Where(d => d.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle=> details.Where(d => d.Title.Contains(text)),
            _ => details,
        };
    }

    public static IQueryable<Detail> DetailOrder(this IQueryable<Detail> details, DetailOrderOptions orderOptions = default, KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            DetailOrderOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.Title),
                KindOfOrder.Down => details.OrderByDescending(d => d.Title),
                _ => details.OrderBy(d => d.Id)
            },
            DetailOrderOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.SerialNumber),
                KindOfOrder.Down => details.OrderByDescending(d => d.SerialNumber),
                _ => details.OrderBy(d => d.Id)
            },
            DetailOrderOptions.ForDetailType=> kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.DetailType!.Title),
                KindOfOrder.Down => details.OrderByDescending(d => d.DetailType!.Title),
                _ => details.OrderBy(d => d.Id)
            },
            _ => details.OrderBy(d => d.Id)
        };
    }

    public static IQueryable<Detail> DetailFromDetailType(this IQueryable<Detail> details, int detailTypeId)
    {
        if (detailTypeId == 0) 
            return details;
        return details.Where(d => d.DetailTypeId == detailTypeId);
    }

    public static IQueryable<Detail> DetailFromProduct(this IQueryable<Detail> details, List<int> detailsId)
    {
        if (detailsId.IsNullOrEmpty())
            return details;
        return details.Where(d => detailsId.Contains(d.Id));
    }

    public static IQueryable<Detail> DetailFromCompound(this IQueryable<Detail> details, IsCompoundDetailOptions options)
    {
        return options switch
        {
            IsCompoundDetailOptions.Compound => details.Where(d => d.DetailsChildren!.Count > 0).Include(d => d.DetailsChildren),
            IsCompoundDetailOptions.NonCompound => details.Where(d => d.DetailsChildren!.Count == 0).Include(d => d.DetailsChildren),
            _ => details,
        };
    }
}