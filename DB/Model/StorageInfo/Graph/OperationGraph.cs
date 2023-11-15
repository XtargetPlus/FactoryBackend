using DB.Model.DetailInfo;
using DB.Model.StatusInfo;
using DB.Model.SubdivisionInfo;
using DB.Model.UserInfo;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Операционный график
/// </summary>
public class OperationGraph : BaseModel, IValidatableObject
{
    /// <summary>
    /// Приоритет графика
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Партия
    /// </summary>
    public float PlanCount { get; set; }

    /// <summary>
    /// На какой месяц создан график
    /// </summary>
    public DateOnly GraphDate { get; set; }

    /// <summary>
    /// Начало графика
    /// </summary>
    public DateOnly? StartDate { get; set; }
    
    /// <summary>
    /// Завершение графика
    /// </summary>
    public DateOnly? FinishDate { get; set; }
    
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Подтвержден ли график
    /// </summary>
    public bool IsConfirmed { get; set; }
    
    /// <summary>
    /// За каким цехом закреплен график
    /// </summary>
    public int SubdivisionId { get; set; }
    public Subdivision? Subdivision { get; set; }
    
    /// <summary>
    /// Владелец графика
    /// </summary>
    public int OwnerId { get; set; }
    public User? Owner { get; set; }
    
    /// <summary>
    /// Подтверждающий графика
    /// </summary>
    public int? ConfigrmingId { get; set; }
    public User? Configrming { get; set; }

    /// <summary>
    /// Статус графика
    /// </summary>
    public int StatusId { get; set; }
    public Status? Status { get; set; }
    
    /// <summary>
    /// Изделие графика
    /// </summary>
    public int? ProductDetailId { get; set; }
    public Detail? ProductDetail { get; set; }

    public List<OperationGraphDetail>? OperationGraphDetails { get; set; }
    public List<OperationGraphGroup>? OperationGraphMainGroups { get; set; }
    public List<OperationGraphGroup>? OperationGraphNextGroups { get; set; }
    public List<OperationGraphUser>? OperationGraphUsers { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Priority < 0)
            yield return new ValidationResult("Приоритет не может быть меньше 0", new[] { nameof(Priority) });

        if (Note?.Length > 500)
            yield return new ValidationResult("Максимальная длина примечания - 500 символов", new[] { nameof(Note) });

        if (PlanCount < 0)
            yield return new ValidationResult("Нужно указать партию", new[] { nameof(PlanCount) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;

        if (context.Set<OperationGraph>().Entry(this) is { State: EntityState.Modified } entity)
        {
            if (entity.Property(nameof(GraphDate)).IsModified && (GraphDate.Month < DateTime.Now.Month || GraphDate.Year < DateTime.Now.Year))
                yield return new ValidationResult("Нельзя указывать прошедший месяц и год", new[] { nameof(GraphDate) });
        }

        if (!context.Set<User>().Any(u => u.Id == OwnerId))
            yield return new ValidationResult("Не удалось найти создателя графика", new[] { nameof(OwnerId) });
        if (!context.Set<Subdivision>().Any(s => s.Id == SubdivisionId))
            yield return new ValidationResult("Не удалось найти подразделение", new[] { nameof(SubdivisionId) });
        if (ProductDetailId is > 0 && !context.Set<Detail>().Any(d => d.Id == ProductDetailId))
            yield return new ValidationResult("Не удалось найти изделие", new[] { nameof(ProductDetailId) });
        if (ConfigrmingId is > 0 && !context.Set<User>().Any(u => u.Id == ConfigrmingId))
            yield return new ValidationResult("Не удалось найти подтверждающего", new[] { nameof(ConfigrmingId) });
        if (!context.Set<Status>().Any(s => s.Id == StatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(StatusId) });
    }
}
