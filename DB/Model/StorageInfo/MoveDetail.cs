using DB.Model.StorageInfo.Graph;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.StorageInfo;

/// <summary>
/// Движение деталей между подразделениями
/// </summary>
public class MoveDetail : BaseModel, IValidatableObject
{
    /// <summary>
    /// Дата движения
    /// </summary>
    public DateTime MoveDate { get; set; }
    /// <summary>
    /// Количество деталей, которые участвуют в перемещении
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Успешно ли выполнялось перемещение
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Склад, с которым происходит взаимодействие
    /// </summary>
    public int StorageId { get; set; }
    public Storage? Storage { get; set; }
    /// <summary>
    /// Тип движение
    /// </summary>
    public int MoveTypeId { get; set; }
    public MoveType? MoveType { get; set; }
    /// <summary>
    /// Инициатор движения
    /// </summary>
    public int InitiatorId { get; set; }
    public User? Initiator { get; set; }
    /// <summary>
    /// По какой операции тех процесса он проходит
    /// </summary>
    public int TechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? TechnologicalProcessItem { get; set; }
    /// <summary>
    /// По какой операции графика детали он проходит
    /// </summary>
    public int? OperationGraphDetailItemId { get; set; }
    public OperationGraphDetailItem? OperationGraphDetailItem { get; set; }

    public List<MoveDetail>? PreviousMovesDetails { get; set; }
    public List<MoveDetail>? NextMovesDetails { get; set; }
    public List<StoragePlaceMoveDetail>? StoragePlaceMoveDetails { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Count < 1)
            yield return new ValidationResult("Количество имеет недопустимое значение", new[] { nameof(Count) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<TechnologicalProcessItem>().Any(tpi => tpi.Id == TechnologicalProcessItemId))
            yield return new ValidationResult("Не удалось найти операцию технического процесса", new[] { nameof(TechnologicalProcessItemId) });
        if (!context.Set<MoveType>().Any(mt => mt.Id == MoveTypeId))
            yield return new ValidationResult("Не удалось найти тип движения", new[] { nameof(MoveTypeId) });
    }
}
 