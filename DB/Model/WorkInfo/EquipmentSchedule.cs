using DB.Model.SubdivisionInfo.EquipmentInfo;

namespace DB.Model.WorkInfo;

public class EquipmentSchedule
{
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool CanDebug { get; set; }
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    public int WorkingPartId { get; set; }
    public WorkingPart? WorkingPart { get; set; }
}
