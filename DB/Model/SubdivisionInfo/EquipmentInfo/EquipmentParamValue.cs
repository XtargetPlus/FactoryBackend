using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Значения параметров станков
/// </summary>
public class EquipmentParamValue : IValidatableObject
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public string Value { get; set; } = null!;
    /// <summary>
    /// Станок
    /// </summary>
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    /// <summary>
    /// Параметр
    /// </summary>
    public int EquipmentParamId { get; set; }
    public EquipmentParam? EquipmentParam { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Value.Length < 1)
            yield return new ValidationResult("Длина значения параметра минимум 1 символ", new[] { nameof(Value) });
        if (Value.Length > 255)
            yield return new ValidationResult("Длина значения параметра максимум 254 символ", new[] { nameof(Value) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Equipment>().Any(e => e.Id == EquipmentId))
            yield return new ValidationResult("Не удалось найти станок", new[] { nameof(EquipmentId) });
        if (!context.Set<EquipmentParam>().Any(ep => ep.Id == EquipmentParamId))
            yield return new ValidationResult("Не удалось найти параметр станка", new[] { nameof(EquipmentParamId) });
    }
}
