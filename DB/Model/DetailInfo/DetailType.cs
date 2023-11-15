using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.DetailInfo;

/// <summary>
/// Тип детали
/// </summary>
public class DetailType : BaseTitleModel, IValidatableObject
{
    public List<Detail>? Details { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина типа детали минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина типа детали максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<DetailType>().Any(m => m.Id != Id && m.Title == Title))
            yield return new ValidationResult("Данный тип детали уже существует", new[] { nameof(Title) });
    }
}