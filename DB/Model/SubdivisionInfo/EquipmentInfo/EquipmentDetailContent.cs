using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Детали станка
/// </summary>
public class EquipmentDetailContent : IValidatableObject, IEquatable<EquipmentDetailContent>
{
    /// <summary>
    /// Станок
    /// </summary>
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    /// <summary>
    /// Деталь
    /// </summary>
    public int EquipmentDetailId { get; set; }
    public EquipmentDetail? EquipmentDetail { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Equipment>().Any(e => e.Id == EquipmentId))
            yield return new ValidationResult("Не удалось найти станок", new[] { nameof(EquipmentId) });
        if (!context.Set<EquipmentDetail>().Any(ed => ed.Id == EquipmentDetailId))
            yield return new ValidationResult("Не удалось найти деталь станка ", new[] { nameof(EquipmentDetailId) });
    }

    public bool Equals(EquipmentDetailContent? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EquipmentId.Equals(other.EquipmentId) && EquipmentDetailId.Equals(other.EquipmentDetailId);
    }

    public override int GetHashCode()
    {
        var hashDetailId = EquipmentDetailId.GetHashCode();
        var hashEquipment = EquipmentId.GetHashCode();
        return hashDetailId ^ hashEquipment;
    }
}
