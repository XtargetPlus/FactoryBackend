using DB.Model.DetailInfo;
using DB.Model.TechnologicalProcessInfo;

namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Операционные графики деталей
/// </summary>
public class OperationGraphDetail : BaseModel
{
    /// <summary>
    /// Плановое количество партии
    /// </summary>
    public float PlannedNumber { get; set; }
    /// <summary>
    /// Суммарное количество партии данной детали во всех составах
    /// </summary>
    public float? TotalPlannedNumber { get; set; }
    /// <summary>
    /// Применяемость в изделии конкретной детали в сборке
    /// </summary>
    public float Usability { get; set; }
    /// <summary>
    /// Применяемость с учетом родителей
    /// </summary>
    public float UsabilityWithFathers { get; set; }
    /// <summary>
    /// Суммарная применяемость всех одинаковых деталей
    /// </summary>
    public float? UsabilitySum { get; set; }
    /// <summary>
    /// Номер графика детали в отоборажении без повторов
    /// </summary>
    public int DetailGraphNumberWithoutRepeats { get; set; }
    /// <summary>
    /// Номер графика детали в отображении с повторами
    /// </summary>
    public string DetailGraphNumberWithRepeats { get; set; } = null!;
    /// <summary>
    /// Общий порядковый номер для детали в графике
    /// </summary>
    public int DetailGraphNumber { get; set; }
    /// <summary>
    /// Количество деталей в потоке
    /// </summary>
    public float? CountInStream { get; set; }
    /// <summary>
    /// Количество деталей, зарезервированных на складе готовой продукции
    /// </summary>
    public float? FinishedGoodsInventory { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Отображается ли график детали в списке
    /// </summary>
    public bool IsVisible { get; set; }
    /// <summary>
    /// Подтверждена ли деталь графика
    /// </summary>
    public bool IsConfirmed { get; set; }
    /// <summary>
    /// Деталь
    /// </summary>
    public int DetailId { get; set; }
    public Detail? Detail { get; set; }
    /// <summary>
    /// Тех процесс детали
    /// </summary>
    public int? TechnologicalProcessId { get; set; }
    public TechnologicalProcess? TechnologicalProcess { get; set; }
    /// <summary>
    /// Операционный график владелец
    /// </summary>
    public int OperationGraphId { get; set; }
    public OperationGraph? OperationGraph { get; set; }

    public List<OperationGraphDetailItem>? OperationGraphDetailItems { get; set; }
    public List<OperationGraphDetailGroup>? OperationGraphMainDetails { get; set; }
    public List<OperationGraphDetailGroup>? OperationGraphNextDetails { get; set; }
}
