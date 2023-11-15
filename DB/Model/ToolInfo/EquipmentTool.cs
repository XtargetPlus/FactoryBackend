using System.ComponentModel.DataAnnotations;
using DB.Model.SubdivisionInfo.EquipmentInfo;

namespace DB.Model.ToolInfo;

public class EquipmentTool : IValidatableObject
{
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    public int ToolId { get; set; }
    public Tool? Tool { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EquipmentId < 1)
            yield return new ValidationResult("Не удалось найти станок");
        if (ToolId < 1)
            yield return new ValidationResult("Не удалось найти инструмент");
    }
}