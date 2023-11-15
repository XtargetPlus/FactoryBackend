using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.AccessoryInfo;

/// <summary>
/// Стороние организации для производства
/// </summary>
public class OutsideOrganization : BaseTitleModel, IValidatableObject
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
            yield return new ValidationResult("Длина наименование минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименование максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<OutsideOrganization>().Any(oo => oo.Id != Id && oo.Title == Title))
            yield return new ValidationResult("Сторонняя организация с таким наименованием уже существует", new[] { nameof(Title) });
    }
}