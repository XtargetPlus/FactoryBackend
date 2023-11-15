namespace Shared.Dto.TechnologicalProcess;

public class EquipmentOperationDto
{
    public int EquipmentId { get; set; }
    public int TechProcessItemId { get; set; }
    public float DebugTime { get; set; }
    public float LeadTime { get; set; }
    public string? Note { get; set; }
}
