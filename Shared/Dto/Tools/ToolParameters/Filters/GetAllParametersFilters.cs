using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools.ToolParameters.Filters;

public class GetAllParametersFilters
{
    [Range(1,int.MaxValue)]
    public int ToolId { get; set; }
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}