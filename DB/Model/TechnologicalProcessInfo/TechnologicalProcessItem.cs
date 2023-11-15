using System.ComponentModel.DataAnnotations;
using DB.Model.AccessoryInfo;
using DB.Model.StorageInfo;
using DB.Model.StorageInfo.Graph;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;

namespace DB.Model.TechnologicalProcessInfo;

/// <summary>
/// Операция технологического процесса
/// </summary>
public class TechnologicalProcessItem : BaseModel, IValidatableObject
{
    /// <summary>
    /// Порядковый номер выполнения
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// Номер операции (Пример - 005, 010)
    /// </summary>
    public string OperationNumber { get; set; } = null!;
    /// <summary>
    /// Приоритет выполнения (Используется в построении ответвлений)
    /// </summary>
    public int Priority { get; set; }
    /// <summary>
    /// Количество изготовляемых единиц за операцию. Сколько получится деталей из 1 заготовки
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Видна ли операция
    /// </summary>
    public bool Show { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// К какому тех процессу принадлежит
    /// </summary>
    public int TechnologicalProcessId { get; set; }
    public TechnologicalProcess? TechnologicalProcess { get; set; }
    /// <summary>
    /// Короткое и полное наименование операции
    /// </summary>
    public int? OperationId { get; set; }
    public Operation? Operation { get; set; }
    /// <summary>
    /// Родительский тех процесс
    /// </summary>
    public int? MainTechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? MainTechnologicalProcessItem { get; set; }
    public List<TechnologicalProcessItem>? BranchesTechnologicalProcessItems { get; set; }
    public List<EquipmentOperation>? EquipmentOperations { get; set; }
    public List<Accessory>? Accessories { get; set; }
    public List<EquipmentPlan>? EquipmentPlans { get; set; }
    public List<MoveDetail>? MoveDetails { get; set; }
    public List<OperationGraphDetailItem>? OperationGraphDetailItems { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Number < 1)
            yield return new ValidationResult("Порядковый номер имеет недопустимое значение", new[] { nameof(Number) });

        if (Priority < 5)
            yield return new ValidationResult("Приоритет имеет недопустимое значение", new[] { nameof(Priority) });

        if (Count < 1)
            yield return new ValidationResult("Количество имеет недопустимое значение", new[] { nameof(Count) });

        if (OperationNumber.Length < 1)
            yield return new ValidationResult("Длина номера операции минимум 1 символ", new[] { nameof(OperationNumber) });
        if (OperationNumber.Length > 255)
            yield return new ValidationResult("Длина номера операции максимум 255 символ", new[] { nameof(OperationNumber) });

        if (Note?.Length < 1)
            yield return new ValidationResult("Длина примечания минимум 1 символ", new[] { nameof(Note) });
        if (Note?.Length > 255)
            yield return new ValidationResult("Длина примечания максимум 255 символ", new[] { nameof(Note) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (OperationId != null && !context.Set<Operation>().Any(o => o.Id == OperationId))
            yield return new ValidationResult("Не удалось найти операцию", new[] { nameof(OperationId) });
        if (!context.Set<TechnologicalProcess>().Any(tp => tp.Id == TechnologicalProcessId))
            yield return new ValidationResult("Не удалось найти технологический процесс", new[] { nameof(TechnologicalProcessId) });
        if (context
            .Set<TechnologicalProcessItem>()
            .IgnoreQueryFilters()
            .Any(tpi => tpi.Id != Id && tpi.TechnologicalProcessId == TechnologicalProcessId && tpi.OperationNumber == OperationNumber))
            yield return new ValidationResult("Операция с таким номером уже существует", new[] { nameof(OperationNumber) });
    }
}
