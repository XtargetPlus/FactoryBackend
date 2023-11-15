using DB.Model.TechnologicalProcessInfo;

namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Операция в графике детали
/// </summary>
public class OperationGraphDetailItem : BaseModel
{
    /// <summary>
    /// Фактическое количество деталей в операции
    /// </summary>
    public float FactCount { get; set; }
    /// <summary>
    /// Номер операции
    /// </summary>
    public int OrdinalNumber { get; set; }
    /// <summary>
    /// Есть ли брак на операции
    /// </summary>
    public bool IsHaveDefective { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// К какому графику детали принадлежит
    /// </summary>
    public int OperationGraphDetailId { get; set; }
    public OperationGraphDetail? OperationGraphDetail { get; set; }
    /// <summary>
    /// Какую операцию тех процесса реализовывает
    /// </summary>
    public int TechnologicalProcessItemId { get; set; }
    public TechnologicalProcessItem? TechnologicalProcessItem { get; set; }
    public List<MoveDetail>? MoveDetails { get; set; }
}
