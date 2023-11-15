using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.StorageInfo;

/// <summary>
/// Тип движения
/// </summary>
public class MoveType : BaseTitleModel, IValidatableObject
{
    public List<MoveDetail>? MoveDetails { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина типа движения минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 20)
            yield return new ValidationResult("Длина типа движения максимум 20 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<MoveType>().Any(m => m.Id != Id && m.Title == Title))
            yield return new ValidationResult("Данный тип движения уже существует", new[] { nameof(Title) });
    }
}
