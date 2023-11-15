using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;
public class ToolParameterConcrete : IValidatableObject
{
    public int ToolId { get; set; }
    public Tool? Tool { get; set; }
    public int ToolParameterId { get; set; }
    public ToolParameter? ToolParameter { get; set; }
    public string Value { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Value.Length < 1)
            yield return new ValidationResult("Значение должно быть больше 1");
        if (Value.Length > 255)
            yield return new ValidationResult("Значение должно быть меньше 255");
    }
}
