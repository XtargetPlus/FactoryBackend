using DB.Model.ToolInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.ToolO;

public static class ToolEquipmnetOptions
{
    public static IQueryable<EquipmentTool> ToolEquipmentOrder(this IQueryable<EquipmentTool> tools,
        ToolReplaceabilitysOptions orderOptions,
        KindOfOrder kindOfOrder)
    {
        return orderOptions switch
        {
            ToolReplaceabilitysOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(et => et.Equipment!.Title),
                KindOfOrder.Down => tools.OrderByDescending(et => et.Equipment!.Title),
                _ => tools.OrderBy(tc => tc.EquipmentId)
            },
            ToolReplaceabilitysOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(et => et.Equipment!.SerialNumber),
                KindOfOrder.Down => tools.OrderByDescending(et => et.Equipment!.SerialNumber),
                _ => tools.OrderBy(tc => tc.EquipmentId)
            },

            _ => tools.OrderBy(et => et.EquipmentId)
        };

    }
}