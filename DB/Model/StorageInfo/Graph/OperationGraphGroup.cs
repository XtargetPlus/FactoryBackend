namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Группа операционных графиков
/// </summary>
public class OperationGraphGroup
{
    /// <summary>
    /// Первый (главный) операционный график
    /// </summary>
    public int OperationGraphMainId { get; set; }
    public OperationGraph? OperationGraphMain { get; set; }
    /// <summary>
    /// Следующий перационный график в группе
    /// </summary>
    public int OperationGraphNextId { get; set; }
    public OperationGraph? OperationGraphNext { get; set; }
}
