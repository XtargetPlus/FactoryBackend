using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.DetailInfo;

/// <summary>
/// Состав детали
/// </summary>
public class DetailsChild : IValidatableObject
{
    /// <summary>
    /// Количество детали в составе
    /// </summary>
    public float Count { get; set; }
    /// <summary>
    /// Порядковый номер при сборке
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// Дочерняя деталь (деталь в составе)
    /// </summary>
    public int ChildId { get; set; }
    public Detail? Child { get; set; }
    /// <summary>
    /// Главная деталь
    /// </summary>
    public int FatherId { get; set; }
    public Detail? Father { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Number < 1)
            yield return new ValidationResult("Порядковый номер имеет недопустимое значение", new[] { nameof(ChildId) });

        if (Count <= 0)
            yield return new ValidationResult("Параметр имеет недопустимое значение", new[] { nameof(ChildId) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Detail>().Any(d => d.Id == ChildId))
            yield return new ValidationResult("Не удалось найти дочернюю деталь", new[] { nameof(ChildId) });
        if (!context.Set<Detail>().Any(d => d.Id == FatherId))
            yield return new ValidationResult("Не удалось найти родительскую деталь", new[] { nameof(FatherId) });
    }
}
