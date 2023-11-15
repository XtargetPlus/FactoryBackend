using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools.ToolChild.Filters;

public class GetAllChildrenFilters
{
    [Range(1, int.MaxValue)]
    public int FatherId { get; set; }
    [DefaultValue(ToolChildOrderOptions.ForNumber)]
    [Range((int)ToolChildOrderOptions.Base,(int)ToolChildOrderOptions.ForSerialNumber)]
    public ToolChildOrderOptions OrderOptions { get; set; }
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base,(int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}