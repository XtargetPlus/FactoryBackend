using DB.Model.TechnologicalProcessInfo;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Операция на станке
/// </summary>
public class EquipmentOperation : IValidatableObject
{
    /// <summary>
    /// Первичный ключ записи
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Время наладки
    /// </summary>
    public float DebugTime { get; set; }
    /// <summary>
    /// Время выполнения на партию
    /// </summary>
    public float LeadTime { get; set; }
    /// <summary>
    /// Приоритет
    /// </summary>
    public byte Priority { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Станок
    /// </summary>
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    /// <summary>
    /// Операция тех процесса
    /// </summary>
    public int TechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? TechnologicalProcessItem { get; set; }

    public List<EquipmentOperationTool>? EquipmentOperationTools { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Note?.Length < 1)
            yield return new ValidationResult("Длина примечания минимум 1 символ", new[] { nameof(Note) });
        if (Note?.Length > 1000)
            yield return new ValidationResult("Длина примечания максимум 1000 символ", new[] { nameof(Note) });

        if (DebugTime < 0 || DebugTime > float.MaxValue)
            yield return new ValidationResult("Время наладки имеет недопустимое значение", new[] { nameof(DebugTime) });

        if (LeadTime < 0 || LeadTime > float.MaxValue)
            yield return new ValidationResult("Время выполнения недопустимое значение", new[] { nameof(LeadTime) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Equipment>().Any(e => e.Id == EquipmentId))
            yield return new ValidationResult("Не удалось найти станок", new[] { nameof(EquipmentId) });
        if (!context.Set<TechnologicalProcessItem>().Any(tpi => tpi.Id == TechnologicalProcessItemId))
            yield return new ValidationResult("Не удалось найти операцию технического процесса", new[] { nameof(TechnologicalProcessItemId) });
    }
}
