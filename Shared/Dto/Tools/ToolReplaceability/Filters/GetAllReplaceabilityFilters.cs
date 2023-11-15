using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools.ToolReplaceability.Filters;

public class GetAllReplaceabilityFilters
{
    [Range(1,int.MaxValue)]
    public int ToolId { get; set; }

    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }

    [DefaultValue(ToolReplaceabilitysOptions.ForTitle)]
    [Range((int)ToolReplaceabilitysOptions.Base, (int)ToolReplaceabilitysOptions.ForSerialNumber)]
    public ToolReplaceabilitysOptions sortOptions { get; set; }
}