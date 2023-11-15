using DB.Model.ToolInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.ToolO;

public static class ToolParametersOption
{
    public static IQueryable<ToolParameterConcrete> ToolParameterOrder(this IQueryable<ToolParameterConcrete> tools,
        KindOfOrder kindOfOrder)
    {
        return kindOfOrder switch
        {
            KindOfOrder.Up => tools.OrderBy(tc => tc.ToolParameter.Title),
            KindOfOrder.Down => tools.OrderByDescending(tc => tc.ToolParameter.Title),
            _ => tools.OrderBy(tc => tc.ToolParameter.Title)
        };
    }
}