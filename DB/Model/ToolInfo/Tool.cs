using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;
public class Tool : BaseTitleModel, IValidatableObject
{
    /// <summary>
    /// Серийный номер
    /// </summary>
    public string? SerialNumber { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }

    public List<ToolChild>? ToolChildren {  get; set; }
    public List<ToolChild>? ToolFathers {  get; set; }

    public List<ToolReplaceability>? Slaves {  get; set; }
    public List<ToolReplaceability>? Masters { get; set; }
    public List<ToolParameterConcrete>? ParametersConcrete { get; set; }
    public List<ToolCatalogConcrete>? ToolCatalogsConcretes { get; set; }
    public List<EquipmentTool>? EquipmentTools { get; set; }
    public List<EquipmentOperationTool>? EquipmentOperationTools { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ");
        if (Title.Length > 500)
            yield return new ValidationResult("Длина наименования максимум 500 символов");
    }
}
