using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Детали на замену или покупку
/// </summary>
public class EquipmentDetailReplacement : IValidatableObject
{
    /// <summary>
    /// Количество деталей
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Детали станка
    /// </summary>
    public int EquipmentDetailId { get; set; }
    public EquipmentDetail? EquipmentDetail { get; set; }
    /// <summary>
    /// Статус по к которому привязаны детали
    /// </summary>
    public int EquipmentStatusValueId { get; set; }
    public EquipmentStatusValue? EquipmentStatusValue { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Count < 1)
            yield return new ValidationResult("Количество имеет недопустимое значение", new[] { nameof(Count) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<EquipmentDetail>().Any(ed => ed.Id == EquipmentDetailId))
            yield return new ValidationResult("Не удалось найти деталь станка", new[] { nameof(EquipmentDetailId) });
        if (!context.Set<EquipmentStatusValue>().Any(esv => esv.Id == EquipmentStatusValueId))
            yield return new ValidationResult("Не удалось найти значение статуса", new[] { nameof(EquipmentStatusValueId) });
    }
}
