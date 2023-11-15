using DB.Model.DetailInfo;
using DB.Model.StatusInfo;
using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DB.Model.StorageInfo.Graph;

namespace DB.Model.TechnologicalProcessInfo;

/// <summary>
/// Технологический процесс
/// </summary>
public class TechnologicalProcess : BaseModel, IValidatableObject
{
    /// <summary>
    /// Дата завершения разработки
    /// </summary>
    public DateOnly FinishDate { get; set; }
    /// <summary>
    /// Приоритет разработки
    /// </summary>
    public byte DevelopmentPriority { get; set; }
    /// <summary>
    /// Актуальный ли тех процесс (можно ли его выдавать и использовать)
    /// </summary>
    public bool IsActual { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Деталь, к которой привязан тех процесс
    /// </summary>
    public int DetailId { get; set; }
    public Detail? Detail { get; set; }
    /// <summary>
    /// Разработчик тех процесса
    /// </summary>
    public int DeveloperId { get; set; }
    public User? Developer { get; set; }
    /// <summary>
    /// Информация о тех процесса
    /// </summary>
    public TechnologicalProcessData TechnologicalProcessData { get; set; } = null!;
    /// <summary>
    /// Приоритет применяемости
    /// </summary>
    public byte ManufacturingPriority { get; set; }
    
    public List<TechnologicalProcessItem>? TechnologicalProcessItems { get; set; }
    public List<TechnologicalProcessStatus>? TechnologicalProcessStatuses { get; set; }
    public List<OperationGraphDetail>? OperationGraphDetails { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ManufacturingPriority < 1)
            yield return new ValidationResult("Приоритет применения не может быть равным или меньше нуля", new[] { nameof(ManufacturingPriority) });

        if (Note?.Length > 1000)
            yield return new ValidationResult("Длина примечания максимум 1000 символ", new[] { nameof(Note) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Detail>().Any(d => d.Id == DetailId))
            yield return new ValidationResult("Не удалось найти деталь", new[] { nameof(DetailId) });
        if (!context.Set<User>().Any(u => u.Id == DeveloperId && u.RoleId == 7))
            yield return new ValidationResult("Попытка назначить технический процесс не существующему или стороннему сотруднику", new[] { nameof(DeveloperId) });
    }
}