using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.TechnologicalProcessInfo;

namespace DB.Model.WorkInfo;

public class EquipmentPlan
{
    public DateTime PlainningData { get; set; }
    public int Count { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public byte WorkingKind { get; set; }
    public int TechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? TechnologicalProcessItem { get; set; }
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
}
