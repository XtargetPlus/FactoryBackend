using DB.Model.DetailInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.DetailO;

public static class DetailReplaceabilityOptions
{
    public static IQueryable<DetailReplaceability> DetailOrder(this IQueryable<DetailReplaceability> details, 
        DetailOrderOptions orderOptions = default, 
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            DetailOrderOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.SecondDetail.Title),
                KindOfOrder.Down => details.OrderByDescending(d => d.SecondDetail.Title),
                _ => details.OrderBy(d => d.SecondDetailId)
            },
            DetailOrderOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => details.OrderBy(d => d.SecondDetail.SerialNumber),
                KindOfOrder.Down => details.OrderByDescending(d => d.SecondDetail.SerialNumber),
                _ => details.OrderBy(d => d.SecondDetailId)
            },
            _ => details.OrderBy(d => d.SecondDetailId)
        };
    }
}