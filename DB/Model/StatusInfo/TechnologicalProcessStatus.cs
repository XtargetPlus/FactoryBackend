using DB.Model.SubdivisionInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.StatusInfo;

/// <summary>
/// Статус тех процесса
/// </summary>
public class TechnologicalProcessStatus : IValidatableObject
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Дата создания статуса
    /// </summary>
    public DateTime StatusDate { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Технический процесс
    /// </summary>
    public int TechnologicalProcessId { get; set; }
    public TechnologicalProcess? TechnologicalProcess { get; set; }
    /// <summary>
    /// Статус
    /// </summary>
    public int StatusId { get; set; }
    public Status? Status { get; set; }
    /// <summary>
    /// Инициатор статуса
    /// </summary>
    public int UserId { get; set; }
    public User? User { get; set; }
    /// <summary>
    /// Подразделение, в которое выдают дубликат тех процесс
    /// </summary>
    public int? SubdivisionId { get; set; }
    public Subdivision? Subdivision { get; set; }

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

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (TechnologicalProcessId > 0 && !context.Set<TechnologicalProcess>().Any(tp => tp.Id == TechnologicalProcessId))
            yield return new ValidationResult("Не удалось найти технический процесс", new[] { nameof(TechnologicalProcessId) });
        if (!context.Set<Status>().Any(s => s.Id == StatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(StatusId) });
        if (!context.Set<User>().Any(u => u.Id == UserId))
            yield return new ValidationResult("Не удалось найти сотрудника", new[] { nameof(UserId) });
    }
}
