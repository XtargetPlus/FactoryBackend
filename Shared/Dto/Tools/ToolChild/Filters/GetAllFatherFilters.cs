using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Shared.Dto.Tools.ToolChild.Filters;

public class GetAllFatherFilters
{
    [Range(1, int.MaxValue)]
    public int ChildId { get; set; }
    [DefaultValue(ToolChildOrderOptions.ForNumber)]
    [Range((int)ToolChildOrderOptions.Base, (int)ToolChildOrderOptions.ForSerialNumber)]
    public ToolChildOrderOptions OrderOptions { get; set; }
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}