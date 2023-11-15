using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ToolInfo;

public class EquipmentOperationTool : IValidatableObject
{
    public int Id {  get; set; }
    //Идентификатор инструмента
    public int ToolId { get; set; }
    public Tool? Tool { get; set; }
    //Идентификатор операции на станке
    public int EquipmentOperationId { get; set; }
    public EquipmentOperation? EquipmentOperation { get; set; }
    /// <summary>
    /// Приоритет
    /// </summary>
    public int Priority { get; set; }
    /// <summary>
    /// Количество
    /// </summary>
    public int Count { get; set; }
    public string? Note { get; set; }
    //Идентификатор родительского инструмента
    public int FatherId { get; set; }
    public EquipmentOperationTool? Father { get; set; }

    public List<EquipmentOperationTool>? Children { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;

        if (!context.Set<Tool>().Any(e => e.Id == ToolId))
            yield return new ValidationResult("Не удалось найти инструмент", new[] { nameof(ToolId) });
        if (!context.Set<EquipmentOperation>().Any(tpi => tpi.Id == EquipmentOperationId))
            yield return new ValidationResult("Не удалось найти операцию на станке", new[] { nameof(EquipmentOperationId) });
        if (Priority < 1)
            yield return new ValidationResult("Не указан приоритет");
        if (Count < 1)
            yield return new ValidationResult("Не указано количество");
        if (Note?.Length > 1000)
            yield return new ValidationResult("Примечание максимум 1000 символов");

    }
}
