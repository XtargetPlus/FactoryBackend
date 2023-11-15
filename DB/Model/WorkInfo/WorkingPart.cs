using DB.Model.SubdivisionInfo.EquipmentInfo;

namespace DB.Model.WorkInfo;

/// <summary>
/// Смена
/// </summary>
public class WorkingPart : BaseModel
{
    /// <summary>
    /// Начало смены
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// Конец смены
    /// </summary>
    public DateTime FinishTime { get; set; }
    /// <summary>
    /// Время работы
    /// </summary>
    public int WorkingTime { get; set; }
    public List<EquipmentSchedule>? EquipmentSchedules { get; set; } 
    public List<EquipmentStatusValue>? EquipmentStatusValues { get; set; }
}
