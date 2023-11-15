using DB.Model.ToolInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.ToolO;

public static class ToolReplaceabilityOptions
{
    public static IQueryable<ToolReplaceability> ToolOrder(this IQueryable<ToolReplaceability> tools,
        ToolReplaceabilitysOptions orderOptions,
        KindOfOrder kindOfOrder)
    {
        return orderOptions switch
        {
            ToolReplaceabilitysOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(d => d.Slave!.Title),
                KindOfOrder.Down => tools.OrderByDescending(d => d.Slave!.Title),
                _ => tools.OrderBy(d => d.SlaveId)
            },
            ToolReplaceabilitysOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(d => d.Slave!.SerialNumber),
                KindOfOrder.Down => tools.OrderByDescending(d => d.Slave!.SerialNumber),
                _ => tools.OrderBy(d => d.SlaveId)
            },
            _ => tools.OrderBy(d => d.SlaveId)
        } ;
    }
}