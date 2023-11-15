using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace DatabaseLayer.Options.EquipmentO;

public static class EquipmentDetailOptions
{
    public static IQueryable<EquipmentDetail> EquipmentDetailSearch(this IQueryable<EquipmentDetail> equipmentDetails, SerialNumberOrTitleFilter searchOptions, string text)
    {
        if (string.IsNullOrEmpty(text))
            return equipmentDetails;
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => equipmentDetails.Where(e => e.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => equipmentDetails.Where(e => e.Title.Contains(text)),
            _ => equipmentDetails,
        };
    }

    public static IQueryable<EquipmentDetail> EquipmentDetailOrder(this IQueryable<EquipmentDetail> equipmentDetails, SerialNumberOrTitleFilter orderOptions = default, KindOfOrder kindOfOrder = default)
    {
        return orderOptions switch
        {
            SerialNumberOrTitleFilter.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => equipmentDetails.OrderBy(e => e.Title),
                KindOfOrder.Down => equipmentDetails.OrderByDescending(e => e.Title),
                _ => equipmentDetails.OrderBy(e => e.Id)
            },
            SerialNumberOrTitleFilter.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => equipmentDetails.OrderBy(e => e.SerialNumber),
                KindOfOrder.Down => equipmentDetails.OrderByDescending(e => e.SerialNumber),
                _ => equipmentDetails.OrderBy(e => e.Id)
            },
            _ => equipmentDetails.OrderBy(e => e.Id)
        };
    }

    public static IQueryable<EquipmentDetail> GetEquipmentDetailsByEquipmentId(this IQueryable<EquipmentDetail> equipmentDetails, int equipmentId)
    {
        if (equipmentId <= 0)
            throw new ArgumentException("Id станка меньше или равно 0", nameof(equipmentId));
        return equipmentDetails.Include(ed => ed.EquipmentDetailContents).Where(ed => ed.EquipmentDetailContents!.FirstOrDefault(edc => edc.EquipmentId == equipmentId) != null);
    }
}