using DB.Model.DetailInfo;
using DB.Model.StatusInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.AccessoryInfo;

/// <summary>
/// Оснаска
/// </summary>
public class Accessory : BaseModel, IValidatableObject
{
    [Required(ErrorMessage = "Обязательное поле")]
    public DateTime TaskDate { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    public DateTime FinishDate { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public string TaskNumber { get; set; } = null!;

    [Required(ErrorMessage = "Обязательное поле")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Обязательное поле")]
    public byte Priority { get; set; }

    [MinLength(1, ErrorMessage = "Длина строки минимум 1 символ")]
    [MaxLength(500, ErrorMessage = "Длина строки максимум 500 символ")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public int TechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? TechnologicalProcessItem { get; set; } 

    [Required(ErrorMessage = "Обязательное поле")]
    public int AccessoryTypeId { get; set; }
    public AccessoryType? AccessoryType { get; set; } 

    [Required(ErrorMessage = "Обязательное поле")]
    public int DeveloperId { get; set; }
    public User? Developer { get; set; } 
    public int? OutsideOrganizationId { get; set; }
    public OutsideOrganization? OutsideOrganization { get; set; }
    public int? DetailId { get; set; }
    public Detail? Detail { get; set; }
    public List<AccessoryEquipment>? AccessoryEquipments { get; set; }
    public List<AccessoryDevelopmentStatus>? AccessoryDevelopmentStatuses { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<TechnologicalProcessItem>().Any(a => a.Id == TechnologicalProcessItemId))
            yield return new ValidationResult("Не удалось найти операцию технического процесса", new[] { nameof(TechnologicalProcessItemId) });
        if (!context.Set<AccessoryType>().Any(e => e.Id == AccessoryTypeId))
            yield return new ValidationResult("Не удалось найти тип оснастки", new[] { nameof(AccessoryTypeId) });
        if (!context.Set<User>().Any(e => e.Id == DeveloperId))
            yield return new ValidationResult("Не удалось найти разработчика", new[] { nameof(DeveloperId) });
        if (OutsideOrganizationId != null && !context.Set<OutsideOrganization>().Any(e => e.Id == OutsideOrganizationId))
            yield return new ValidationResult("Не удалось найти стороннюю организацию", new[] { nameof(OutsideOrganizationId) });
        if (DetailId != null && !context.Set<Detail>().Any(e => e.Id == DetailId))
            yield return new ValidationResult("Не удалось найти деталь", new[] { nameof(DetailId) });
    }
}
