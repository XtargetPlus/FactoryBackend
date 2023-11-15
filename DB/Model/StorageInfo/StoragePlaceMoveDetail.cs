namespace DB.Model.StorageInfo;

/// <summary>
/// Хранение деталей во время движения на физических складах
/// </summary>
public class StoragePlaceMoveDetail
{
    /// <summary>
    /// Физическое место хранения
    /// </summary>
    public int StoragePlaceId { get; set; }
    public StoragePlace? StoragePlace { get; set; }
    /// <summary>
    /// По какому движению оно здесь хранится
    /// </summary>
    public int MoveDetailId { get; set; }
    public MoveDetail? MoveDetail { get; set; }
    /// <summary>
    /// Количество деталей
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; } 
}
