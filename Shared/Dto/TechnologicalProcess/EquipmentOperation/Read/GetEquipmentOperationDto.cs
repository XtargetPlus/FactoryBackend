namespace Shared.Dto.TechnologicalProcess;

public class GetEquipmentOperationDto
{
    public int EquipmentOperationId { get; set; }
    public int EquipmentId { get; set; }
    public byte Priority { get; set; }
    public string EquipmentSerialNumber { get; set; } = default!;
    public string EquipmentTitle { get; set; } = default!;
    public string Subdivision { get; set; } = default!;
    public float DebugTime { get; set; }
    public float LeadTime { get; set; }
    public string? Note { get; set; }
}
