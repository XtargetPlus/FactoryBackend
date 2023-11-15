namespace Shared.Dto.TechnologicalProcess.EquipmentOperation.CUD;

public class InsertBetweenEquipmentOperation
{
    public int TechProcessItemId { get; set; }
    public int BeforePriority { get; set; }
    public int CurrentTargetPriority { get; set; }
}
