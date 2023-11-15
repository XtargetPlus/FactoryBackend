using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;

/// <summary>
/// Тип заготовки
/// </summary>
public class BlankType : BaseTitleModel, IValidatableObject
{
    public List<TechnologicalProcessData>? TechnologicalProcessData { get; set; } 

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
        
        if (context.Set<BlankType>().Any(bt => bt.Id != Id && bt.Title == Title))
            yield return new ValidationResult("Данный тип заготовки уже существует", new[] { nameof(Title) });
    }
}