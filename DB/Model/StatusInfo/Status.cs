using DB.Model.StorageInfo.Graph;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.UserInfo;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.StatusInfo;

/// <summary>
/// Статус
/// </summary>
public class Status : BaseModel
{
    /// <summary>
    /// Наименование
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина строки минимум 1 символ")]
    [MaxLength(150, ErrorMessage = "Длина строки максимум 150 символ")]
    public string Title { get; set; } = null!;
    /// <summary>
    /// К какой таблице относится статус
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина строки минимум 1 символ")]
    [MaxLength(50, ErrorMessage = "Длина строки максимум 50 символ")]
    public string TableName { get; set; } = null!;
    public List<AccessoryDevelopmentStatus>? AccessoryDevelopmentStatuses { get; set; }
    public List<TechnologicalProcessStatus>? TechnologicalProcessStatuses { get; set; }
    public List<User>? Users { get; set; }
    public List<EquipmentStatus>? EquipmentStatuses { get; set; }
    public List<OperationGraph>? OperationGraphs { get; set; }
}
