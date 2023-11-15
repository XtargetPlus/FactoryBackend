using DB.Model.AccessoryInfo;
using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DB.Model.ToolInfo;

namespace DB.Model.SubdivisionInfo.EquipmentInfo;

/// <summary>
/// Станок
/// </summary>
public class Equipment : BaseModel, IValidatableObject
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Серийный номер
    /// </summary>
    public string SerialNumber { get; set; } = null!;
    /// <summary>
    /// Подразделение, к которому привяза станок
    /// </summary>
    public int SubdivisionId { get; set; }
    public Subdivision? Subdivision { get; set; }
    public List<EquipmentOperation>? EquipmentOperations { get; set; } 
    public List<AccessoryEquipment>? AccessoryEquipments { get; set; } 
    public List<EquipmentSchedule>? EquipmentSchedules { get; set; } 
    public List<EquipmentPlan>? EquipmentPlans { get; set; }
    public List<EquipmentDetailContent>? EquipmentDetailContents { get; set; }
    public List<EquipmentStatusValue>? EquipmentStatusValues { get; set; }
    public List<EquipmentParamValue>? EquipmentParamValues { get; set; }
    public List<EquipmentTool>? Tools { get; set; }


    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SerialNumber.Length < 1)
            yield return new ValidationResult("Длина серийного номера минимум 1 символ", new[] { nameof(SerialNumber) });
        if (SerialNumber.Length > 50)
            yield return new ValidationResult("Длина серийного номера максимум 50 символ", new[] { nameof(SerialNumber) });

        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 100)
            yield return new ValidationResult("Длина наименования максимум 100 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<Equipment>().Any(e => e.Id != Id && e.SerialNumber == SerialNumber))
            yield return new ValidationResult("Станок с таким серийный номером уже существует", new[] { nameof(SerialNumber) });
        if (!context.Set<Subdivision>().Any(s => s.Id == SubdivisionId))
            yield return new ValidationResult("Не удалось найти подразделение", new[] { nameof(SubdivisionId) });
    }
}