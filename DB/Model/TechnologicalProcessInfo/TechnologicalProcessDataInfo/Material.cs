using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;

/// <summary>
/// Материал
/// </summary>
public class Material : BaseTitleModel, IValidatableObject
{
    public List<TechnologicalProcessData> TechnologicalProcessData { get; set; } = new();

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина материала минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 50)
            yield return new ValidationResult("Длина материала максимум 50 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<Material>().Any(m => m.Id != Id && m.Title == Title))
            yield return new ValidationResult("Данный материал уже существует", new[] { nameof(Title) });
    }
}