using DB.Model.DetailInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.DetailO;

public static class DetailChildOptions
{
    public static IQueryable<DetailsChild> DetailChildOrder(this IQueryable<DetailsChild> details, 
        DetailChildOrderOptions orderOptions = default, 
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            DetailChildOrderOptions.ForNumber => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.Number),
                KindOfOrder.Down => details.OrderByDescending(d => d.Number),
                _ => details.OrderBy(d => d.Number)
            },
            DetailChildOrderOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.Child!.Title),
                KindOfOrder.Down => details.OrderByDescending(d => d.Child!.Title),
                _ => details.OrderBy(d => d.Number)
            },
            DetailChildOrderOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.Child!.SerialNumber),
                KindOfOrder.Down => details.OrderByDescending(d => d.Child!.SerialNumber),
                _ => details.OrderBy(d => d.Number)
            },
            _ => details.OrderBy(d => d.Number)
        };
    }
}