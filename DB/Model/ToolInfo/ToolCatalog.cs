using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;

public class ToolCatalog : BaseTitleModel, IValidatableObject
{
    /// <summary>
    /// Идентификатор родительского каталога
    /// </summary>
    public int? FatherId { get; set; }
    public ToolCatalog? Father { get; set; }

    public List<ToolCatalog>? ChildrenToolCatalogs { get; set; }
    public List<ToolCatalogConcrete>? ToolCatalogsConcretes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Название каталога должно быть больше 1");
        if (Title.Length > 255)
            yield return new ValidationResult("Название каталога должно быть меньше 255");
    }
}
