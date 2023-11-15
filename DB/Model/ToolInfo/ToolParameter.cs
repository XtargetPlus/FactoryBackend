using DB.Model.DetailInfo;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;

public class ToolParameter : BaseTitleModel, IValidatableObject
{
    public int? UnitId {  get; set; }
    public Unit? Unit { get; set; }
    public List<ToolParameterConcrete>? ParametersConcrete { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина названия должна быть больше 1");
        if (Title.Length > 255)
            yield return new ValidationResult("Длина названия должна быть больше 255");
    }
}
