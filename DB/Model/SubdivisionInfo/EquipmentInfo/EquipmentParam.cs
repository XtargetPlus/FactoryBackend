using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Параметры станков
/// </summary>
public class EquipmentParam : BaseModel
{
    /// <summary>
    /// Наименование
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина строки минимум 1 символ")]
    [MaxLength(200, ErrorMessage = "Длина строки максимум 200 символ")]
    public string Title { get; set; } = null!;
    public List<EquipmentParamValue>? EquipmentParamValues { get; set; }
}
