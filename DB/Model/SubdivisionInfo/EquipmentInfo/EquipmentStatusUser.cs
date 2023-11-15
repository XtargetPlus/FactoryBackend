using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Какие статусы какие пользователи могут видеть
/// </summary>
public class EquipmentStatusUser : IValidatableObject
{
    /// <summary>
    /// Статус станка
    /// </summary>
    public int EquipmentStatusId { get; set; }
    public EquipmentStatus? EquipmentStatus { get; set; }
    /// <summary>
    /// Сотрудник
    /// </summary>
    public int UserId { get; set; }
    public User? User { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<User>().Any(u => u.Id == UserId))
            yield return new ValidationResult("Не удалось найти сотрудника", new[] { nameof(UserId) });
        if (!context.Set<EquipmentStatus>().Any(es => es.Id == EquipmentStatusId))
            yield return new ValidationResult("Не удалось найти статус", new[] { nameof(EquipmentStatusId) });
    }
}
