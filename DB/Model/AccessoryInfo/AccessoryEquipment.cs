using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.AccessoryInfo;

/// <summary>
/// Использование оснастки в операции
/// </summary>
public class AccessoryEquipment : IValidatableObject
{
    /// <summary>
    /// Оснастка
    /// </summary>
    public int AccessoryId { get; set; }
    public Accessory? Accessory { get; set; }
    /// <summary>
    /// Станок
    /// </summary>
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Accessory>().Any(a => a.Id == AccessoryId))
            yield return new ValidationResult("Не удалось найти оснастку", new[] { nameof(AccessoryId) });
        if (!context.Set<Equipment>().Any(e => e.Id == EquipmentId))
            yield return new ValidationResult("Не удалось найти станок", new[] { nameof(EquipmentId) });
    }
}
