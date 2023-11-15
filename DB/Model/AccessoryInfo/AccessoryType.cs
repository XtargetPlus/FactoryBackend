using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.AccessoryInfo;

/// <summary>
/// Тип оснастки
/// </summary>
public class AccessoryType : BaseTitleModel, IValidatableObject
{
    public List<Accessory>? Accessories { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина типа оснастки минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 50)
            yield return new ValidationResult("Длина типа оснастки максимум 50 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<AccessoryType>().Any(m => m.Id != Id && m.Title == Title))
            yield return new ValidationResult("Данный тип оснастки уже существует", new[] { nameof(Title) });
    }
}