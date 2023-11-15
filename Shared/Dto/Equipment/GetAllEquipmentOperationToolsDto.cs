using DB.Model.ToolInfo;

namespace Shared.Dto.Equipment;

public class GetAllEquipmentOperationToolsDto
{
    public int EquipmentOperationId { get; set; }
    public List<List<Tool>> Tools { get; set; }
}
