using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.DetailInfo;

/// <summary>
/// Заменяемость детали
/// </summary>
public class DetailReplaceability : IValidatableObject
{
    /// <summary>
    /// Первая деталь
    /// </summary>
    public int FirstDetailId { get; set; }
    public Detail? FirstDetail { get; set; }
    /// <summary>
    /// Вторая деталь
    /// </summary>
    public int SecondDetailId { get; set; }
    public Detail? SecondDetail { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Detail>().Any(d => d.Id == FirstDetailId))
            yield return new ValidationResult("Не удалось найти первую деталь в заменяемости", new[] { nameof(FirstDetailId) });
        if (!context.Set<Detail>().Any(d => d.Id == SecondDetailId))
            yield return new ValidationResult("Не удалось найти вторую деталь в заменяемости", new[] { nameof(SecondDetailId) });
    }
}
