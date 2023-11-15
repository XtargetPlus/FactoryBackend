using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Деталь станка
/// </summary>
public class EquipmentDetail : BaseTitleModel, IValidatableObject
{
    /// <summary>
    /// Серийный номер детали
    /// </summary>
    public string SerialNumber { get; set; } = null!;
    public List<EquipmentDetailContent>? EquipmentDetailContents { get; set; }
    public List<EquipmentDetailReplacement>? EquipmentDetailReplacements { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SerialNumber.Length < 1)
            yield return new ValidationResult("Длина серийного номера минимум 1 символ", new[] { nameof(SerialNumber) });
        if (SerialNumber.Length > 100)
            yield return new ValidationResult("Длина серийного номера максимум 100 символ", new[] { nameof(SerialNumber) });

        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименования максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<EquipmentDetail>().Any(ed => ed.Id != Id && ed.SerialNumber == SerialNumber))
            yield return new ValidationResult("Деталь с таким серийным номером уже существует ", new[] { nameof(SerialNumber) });
    }
}
