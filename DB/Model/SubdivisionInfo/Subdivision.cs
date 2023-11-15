using DB.Model.StatusInfo;
using DB.Model.StorageInfo;
using DB.Model.StorageInfo.Graph;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.SubdivisionInfo;

/// <summary>
/// Подразделение
/// </summary>
public class Subdivision : BaseTitleModel, IValidatableObject
{
    /// <summary>
    /// Родительское подразделение
    /// </summary>
    public int? FatherId { get; set; }
    public Subdivision? Father { get; set; }
    public List<Subdivision>? Subdivisions { get; set; }
    public List<Equipment>? Equipments { get; set; }
    public List<User>? Users { get; set; }
    public List<TechnologicalProcessStatus>? TechnologicalProcessStatuses { get; set; }
    public List<OperationGraph>? OperationGraphs { get; set; }
    public List<Storage>? Storages { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименования максимум 255 символ", new[] { nameof(Title) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (FatherId is > 0 && !context.Set<Subdivision>().Any(s => s.Id == FatherId))
            yield return new ValidationResult("Не удалось найти родительское подразделение", new[] { nameof(FatherId) });
        
        switch (FatherId)
        {
            case null when context.Set<Subdivision>().Any(s => s.Id != Id && s.FatherId == null && s.Title == Title):
                yield return new ValidationResult("Подразделение уже существует на текущем уровне", new[] { nameof(FatherId), nameof(Title) });
                break;
            case <= 0:
                yield return new ValidationResult("FatherId < 0", new[] { nameof(FatherId) });
                break;
        }
    }
}