using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;

/// <summary>
/// Информация о тех процессе
/// </summary>
public class TechnologicalProcessData : BaseModel, IValidatableObject
{
    /// <summary>
    /// Норма расхода
    /// </summary>
    public string? Rate { get; set; } = null!;
    /// <summary>
    /// Тех процесс
    /// </summary>
    public int TecnologicalProcessId { get; set; }
    public TechnologicalProcess? TecnologicalProcess { get; set; }
    /// <summary>
    /// Тип заготовки
    /// </summary>
    public int? BlankTypeId { get; set; }
    public BlankType? BlankType { get; set; }
    /// <summary>
    /// Материал
    /// </summary>
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Rate?.Length < 1)
            yield return new ValidationResult("Длина нормы расхода минимум 1 символ", new[] { nameof(Rate) });
        if (Rate?.Length > 255)
            yield return new ValidationResult("Длина нормы расхода максимум 255 символ", new[] { nameof(Rate) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (BlankTypeId != null && !context.Set<BlankType>().Any(bt => bt.Id ==  BlankTypeId))
            yield return new ValidationResult("Не удалось найти тип заготовки", new[] { nameof(BlankTypeId) });
        if (MaterialId != null && !context.Set<Material>().Any(m => m.Id == MaterialId))
            yield return new ValidationResult("Не удалось найти материал", new[] { nameof(MaterialId) });
        if (TecnologicalProcessId > 0 && !context.Set<TechnologicalProcess>().Any(tp => tp.Id == TecnologicalProcessId))
            yield return new ValidationResult("Не удалось найти технический процесс", new[] { nameof(TecnologicalProcessId) });
    }
}