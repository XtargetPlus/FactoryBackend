using DB.Model.AccessoryInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.StatusInfo;

/// <summary>
/// Статус производства приспособления
/// </summary>
public class AccessoryDevelopmentStatus : IValidatableObject
{
    /// <summary>
    /// Дата создания статуса
    /// </summary>
    public DateTime StatusDate { get; set; }
    /// <summary>
    /// Номер по документу
    /// </summary>
    public string DocumentNumber { get; set; } = null!;
    /// <summary>
    /// Оснастка
    /// </summary>
    public int AccessoryId { get; set; }
    public Accessory? Accessory { get; set; }
    /// <summary>
    /// Разработчик
    /// </summary>
    public int UserId { get; set; }
    public User? User { get; set; }
    /// <summary>
    /// Статус
    /// </summary>
    public int StatusId { get; set; }
    public Status? Status { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DocumentNumber.Length < 1)
            yield return new ValidationResult("Длина номера по документу минимум 1 символ", new[] { nameof(DocumentNumber) });
        if (DocumentNumber.Length > 50)
            yield return new ValidationResult("Длина номера по документу максимум 50 символ", new[] { nameof(DocumentNumber) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Accessory>().Any(tp => tp.Id == AccessoryId))
            yield return new ValidationResult("Не удалось найти оснастку", new[] { nameof(AccessoryId) });
        if (!context.Set<Status>().Any(s => s.Id == StatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(StatusId) });
        if (!context.Set<User>().Any(u => u.Id == UserId))
            yield return new ValidationResult("Не удалось найти сотрудника", new[] { nameof(UserId) });
    }
}
