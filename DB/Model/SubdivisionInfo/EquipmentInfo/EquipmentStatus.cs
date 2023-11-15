using DB.Model.StatusInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Статусы станков
/// </summary>
public class EquipmentStatus : BaseModel, IValidatableObject
{
    /// <summary>
    /// Смена
    /// </summary>
    public bool WorkingPart { get; set; }
    /// <summary>
    /// Причина
    /// </summary>
    public bool Failure { get; set; }
    /// <summary>
    /// Смено-суточное задание
    /// </summary>
    public bool DailyTask { get; set; }
    /// <summary>
    /// Деталь станка
    /// </summary>
    public bool EquipmentDetail { get; set; }
    /// <summary>
    /// Детали в закупке
    /// </summary>
    public bool PurchaseDetail { get; set; }
    /// <summary>
    /// Дата завершения статуса
    /// </summary>
    public bool FinishDate { get; set; }
    /// <summary>
    /// Статус
    /// </summary>
    public int StatusId { get; set; }
    public Status? Status { get; set; }
    public List<EquipmentStatusValue>? EquipmentStatusValues { get; set; }
    public List<EquipmentStatusUser>? EquipmentStatusUsers { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Status>().Any(s => s.Id == StatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(StatusId) });
    }
}
