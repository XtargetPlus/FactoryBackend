using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools.ToolEquipment.Filters;

public class GetAllEquipmentFilters
{
    [Range(1,int.MaxValue)]
    public int Id { get; set; }
    [DefaultValue(ToolReplaceabilitysOptions.ForTitle)]
    [Range((int)ToolReplaceabilitysOptions.Base,(int)ToolReplaceabilitysOptions.ForSerialNumber)]
    public ToolReplaceabilitysOptions OrderOption { get; set; }
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base,(int)KindOfOrder.Down)]
    public KindOfOrder kindOfOrder { get; set; }

}