namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Группа операционных графиков деталей
/// </summary>
public class OperationGraphDetailGroup
{
    /// <summary>
    /// Первый (главный) график в группе
    /// </summary>
    public int OperationGraphMainDetailId { get; set; }
    public OperationGraphDetail? OperationGraphMainDetail { get; set; }
    /// <summary>
    /// Следующий график в группе
    /// </summary>
    public int OperationGraphNextDetailId { get; set; }
    public OperationGraphDetail? OperationGraphNextDetail { get; set; }
}
