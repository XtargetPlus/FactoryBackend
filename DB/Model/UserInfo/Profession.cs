using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.UserInfo;

/// <summary>
/// Профессия
/// </summary>
public class Profession : BaseTitleModel, IValidatableObject
{
    public List<User> Users { get; set; } = new();

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименования максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<Profession>().Any(p => p.Id != Id && p.Title == Title))
            yield return new ValidationResult("Профессия с данным наименованием уже существует", new[] { nameof(Title) });
    }
}