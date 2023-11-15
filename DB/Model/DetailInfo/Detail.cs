using DB.Model.AccessoryInfo;
using DB.Model.ProductInfo;
using DB.Model.StorageInfo.Graph;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.DetailInfo;

/// <summary>
/// Деталь
/// </summary>
public class Detail : BaseTitleModel, IValidatableObject
{
    /// <summary>
    /// Серийный номер
    /// </summary>
    public string SerialNumber { get; set; } = null!;
    /// <summary>
    /// Вес
    /// </summary>
    public float Weight { get; set; }
    /// <summary>
    /// Тип детали
    /// </summary>
    public int DetailTypeId { get; set; }
    public DetailType? DetailType { get; set; }
    /// <summary>
    /// Единица измерения
    /// </summary>
    public int UnitId { get; set; }
    public Unit? Unit { get; set; }
    public List<DetailReplaceability>? Ins { get; set; }
    public List<DetailReplaceability>? Outs { get; set; }
    public List<Product>? Products { get; set; }
    public List<DetailsChild>? DetailsChildren { get; set; }
    public List<DetailsChild>? DetailsFathers { get; set; }
    public List<TechnologicalProcess>? TechnologicalProcesses { get; set; } 
    public List<Accessory>? Accessories { get; set; }
    public List<OperationGraph>? OperationGraphs { get; set; }
    public List<OperationGraphDetail>? OperationGraphDetails { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Weight < 0)
            yield return new ValidationResult("Вес имеет недопустимое значение", new[] { nameof(Weight) });

        if (SerialNumber.Length < 1)
            yield return new ValidationResult("Длина серийного номера минимум 1 символ", new[] { nameof(SerialNumber) });
        if (SerialNumber.Length > 100)
            yield return new ValidationResult("Длина серийного номера максимум 100 символ", new[] { nameof(SerialNumber) });
        
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименования максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<DetailType>().Any(dt => dt.Id == DetailTypeId))
            yield return new ValidationResult("Не удалось найти тип детали", new[] { nameof(DetailTypeId) });
        if (!context.Set<Unit>().Any(u => u.Id == UnitId))
            yield return new ValidationResult("Не удалось найти единицу измерения", new[] { nameof(UnitId) });
        if (context.Set<Detail>().Any(d => d.Id != Id && SerialNumber.Length > 0 && d.SerialNumber == SerialNumber))
            yield return new ValidationResult("Деталь с таким серийным номером уже существует", new[] { nameof(SerialNumber) });
    }
}