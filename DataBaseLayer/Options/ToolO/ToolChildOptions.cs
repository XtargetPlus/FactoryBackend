using DB.Model.ToolInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.ToolO;

public static class ToolChildOptions
{
    public static IQueryable<ToolChild> ToolChildOrder(this IQueryable<ToolChild> tools,
        ToolChildOrderOptions orderOptions,
        KindOfOrder kindOfOrder)
    {
        return orderOptions switch
        {
            ToolChildOrderOptions.ForNumber => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(tc => tc.Priority),
                KindOfOrder.Down => tools.OrderByDescending(tc => tc.Priority),
                _ => tools.OrderBy(tc => tc.Priority)
            },
            ToolChildOrderOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(tc => tc.Child!.SerialNumber),
                KindOfOrder.Down => tools.OrderByDescending(tc => tc.Child!.SerialNumber),
                _ => tools.OrderBy(tc => tc.Priority)
            },
            ToolChildOrderOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => tools.OrderBy(tc => tc.Child!.Title),
                KindOfOrder.Down => tools.OrderByDescending(tc => tc.Child!.Title),
                _ => tools.OrderBy(tc => tc.Priority)
            },
            _ => tools.OrderBy(tc => tc.Priority)
        };
    }
}