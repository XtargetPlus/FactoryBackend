using DB.Model.UserInfo;
using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Значение статуса
/// </summary>
public class EquipmentStatusValue : BaseModel, IValidatableObject
{
    /// <summary>
    /// Начало статуса
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Можно ли планировать на станок
    /// </summary>
    public bool IsPossibleToPlan { get; set; }
    /// <summary>
    /// Инициатор статуса
    /// </summary>
    public int UserId { get; set; }
    public User? User { get; set; }
    /// <summary>
    /// Статус станка
    /// </summary>
    public int EquipmentStatusId { get; set; }
    public EquipmentStatus? EquipmentStatus { get; set; }
    /// <summary>
    /// Станок
    /// </summary>
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    /// <summary>
    /// Смена
    /// </summary>
    public int? WorkingPartId { get; set; }
    public WorkingPart? WorkingPart { get; set; }
    /// <summary>
    /// Причина поломки
    /// </summary>
    public int? EquipmentFaulureId { get; set; }
    public EquipmentFailure? EquipmentFailure { get; set; }
    /// <summary>
    /// Смено-суточное задание
    /// </summary>
    public int? DailyTaskId { get; set; }
    public List<EquipmentDetailReplacement>? EquipmentDetailReplacements { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Note?.Length < 1)
            yield return new ValidationResult("Длина примечания минимум 1 символ", new[] { nameof(Note) });
        if (Note?.Length > 500)
            yield return new ValidationResult("Длина примечания максимум 500 символ", new[] { nameof(Note) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<User>().Any(u => u.Id == UserId))
            yield return new ValidationResult("Не удалось найти пользователя", new[] { nameof(UserId) });
        if (!context.Set<EquipmentStatus>().Any(es => es.Id == EquipmentStatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(EquipmentStatusId) });
        if (!context.Set<Equipment>().Any(e => e.Id == EquipmentId))    
            yield return new ValidationResult("Не удалось найти станок", new[] { nameof(EquipmentId) });
        if (WorkingPartId != null && !context.Set<WorkingPart>().Any(wp => wp.Id == WorkingPartId))
            yield return new ValidationResult("Не удалось найти смену", new[] { nameof(WorkingPartId) });
        if (EquipmentFaulureId != null && !context.Set<EquipmentFailure>().Any(ef => ef.Id == EquipmentFaulureId))
            yield return new ValidationResult("Не удалось найти причину поломки", new[] { nameof(EquipmentFaulureId) });
    }
}
