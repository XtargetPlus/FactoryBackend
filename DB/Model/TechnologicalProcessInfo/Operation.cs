using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.TechnologicalProcessInfo;

/// <summary>
/// Операция
/// </summary>
public class Operation : BaseModel, IValidatableObject
{
    /// <summary>
    /// Полное наименование операции
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина полного наименования минимум 1 символ")]
    [MaxLength(255, ErrorMessage = "Длина полного наименования минимум 255 символ")]
    public string FullName { get; set; } = null!;
    /// <summary>
    /// Короткое наименование операции
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина короткого наименования минимум 1 символ")]
    [MaxLength(100, ErrorMessage = "Длина короткого наименования минимум 100 символ")]
    public string ShortName { get; set; } = null!;
    public List<TechnologicalProcessItem> TechnologicalProcessItems { get; set; } = new();

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<Operation>().Any(o => o.Id != Id && o.FullName == FullName && o.ShortName == ShortName))
            yield return new ValidationResult("Данная операция уже существует", new[] { nameof(ShortName), nameof(FullName) });
    }
}