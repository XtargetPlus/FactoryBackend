using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;

public class ToolReplaceability : IValidatableObject
{
    /// <summary>
    /// Заменяемый инструмент
    /// </summary>
    public int MasterId { get; set; }
    public Tool? Master { get; set; }
    /// <summary>
    /// Заменяющий инструмент
    /// </summary>
    public int SlaveId { get; set; }
    public Tool? Slave { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;

        if (!context.Set<Tool>().Any(d => d.Id == MasterId))
            yield return new ValidationResult("Не удалось найти первый инструмент в заменяемости", new[] { nameof(MasterId) });
        if (!context.Set<Tool>().Any(d => d.Id == SlaveId))
            yield return new ValidationResult("Не удалось найти второй инструмент в заменяемости", new[] { nameof(SlaveId) });
    }
}
