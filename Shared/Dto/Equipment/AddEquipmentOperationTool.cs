namespace Shared.Dto.Equipment;

public class AddEquipmentOperationTool
{
    public int EquipmentOperationId { get; set; }
    public int Count { get; set; }
    public int Priority { get; set; }
    public List<int> Tools { get; set; }
}
