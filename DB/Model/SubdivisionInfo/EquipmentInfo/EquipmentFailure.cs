using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Причина поломки
/// </summary>
public class EquipmentFailure : BaseTitleModel, IValidatableObject
{
    public List<EquipmentStatusValue>? EquipmentStatusValues { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 150)
            yield return new ValidationResult("Длина причины поломки максимум 150 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<EquipmentFailure>().Any(bt => bt.Id != Id && bt.Title == Title))
            yield return new ValidationResult("Данная причина поломки уже существует", new[] { nameof(Title) });
    }
}
