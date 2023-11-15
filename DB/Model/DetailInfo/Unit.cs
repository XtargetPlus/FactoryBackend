using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DB.Model.ToolInfo;

namespace DB.Model.DetailInfo;

/// <summary>
/// Единица измерения
/// </summary>
public class Unit : BaseTitleModel, IValidatableObject
{
    public List<Detail>? Details { get; set; }
    public List<ToolParameter>? ToolParameters { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина единицы измерения минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 50)
            yield return new ValidationResult("Длина единицы измерения максимум 50 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<Unit>().Any(m => m.Id != Id && m.Title == Title))
            yield return new ValidationResult("Данная единица измерения уже существует", new[] { nameof(Title) });
    }
}
