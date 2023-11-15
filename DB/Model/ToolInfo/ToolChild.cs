using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;

public class ToolChild : IValidatableObject
{
    /// <summary>
    /// Идентификатор родительского инструмента
    /// </summary>
    public int FatherId { get; set; }
    public Tool? Father { get; set; }
    /// <summary>
    /// Идентификатор дочернего инструмента
    /// </summary>
    public int ChildId {  get; set; }
    public Tool? Child {  get; set; }
    /// <summary>
    /// Количество
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Приоритет
    /// </summary>
    public int Priority { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Count < 1)
            yield return new ValidationResult("Параметр имеет недопустимое значение");

        if (Priority < 1)
            yield return new ValidationResult("Указано недопустимое значение приоритета");

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;

        if (!context.Set<Tool>().Any(d => d.Id == ChildId))
            yield return new ValidationResult("Не удалось найти дочерний инструмент", new[] { nameof(ChildId) });
        if (!context.Set<Tool>().Any(d => d.Id == FatherId))
            yield return new ValidationResult("Не удалось найти родительский инструмент", new[] { nameof(FatherId) });
    }
}
