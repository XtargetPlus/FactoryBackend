using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Enums;

namespace DatabaseLayer.Options;

public static class EquipmentOptions
{
    public static IQueryable<Equipment> EquipmentSearch(this IQueryable<Equipment> equipments, SerialNumberOrTitleFilter searchOptions, string text)
    {
        if (string.IsNullOrEmpty(text))
            return equipments;
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => equipments.Where(e => e.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => equipments.Where(e => e.Title.Contains(text)),
            _ => equipments,
        };
    }

    public static IQueryable<Equipment> EquipmentOrder(this IQueryable<Equipment> equipments, SerialNumberOrTitleFilter orderOptions = default, KindOfOrder kindOfOrder = default)
    {
        return orderOptions switch
        {
            SerialNumberOrTitleFilter.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => equipments.OrderBy(e => e.Title),
                KindOfOrder.Down => equipments.OrderByDescending(e => e.Title),
                _ => equipments.OrderBy(e => e.Id)
            },
            SerialNumberOrTitleFilter.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => equipments.OrderBy(e => e.SerialNumber),
                KindOfOrder.Down => equipments.OrderByDescending(e => e.SerialNumber),
                _ => equipments.OrderBy(e => e.Id)
            },
            _ => equipments.OrderBy(e => e.Id)
        };
    }

    public static IQueryable<Equipment> SubdivisionOption(this IQueryable<Equipment> equipments, int subdivId)
    {
        if (subdivId <= 0)
            return equipments;
        return equipments.Where(e => e.SubdivisionId == subdivId);
    }
}