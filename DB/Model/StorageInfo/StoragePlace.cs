namespace DB.Model.StorageInfo;

/// <summary>
/// Физическое место хранения
/// </summary>
public class StoragePlace : BaseModel
{
    /// <summary>
    /// Ряд
    /// </summary>
    public int Row { get; set; }
    /// <summary>
    /// Полка
    /// </summary>
    public int Shelf { get; set; }
    /// <summary>
    /// Место
    /// </summary>
    public int Place { get; set; }
    /// <summary>
    /// Ячейка
    /// </summary>
    public int? Cell { get; set; }
    /// <summary>
    /// К какому складу относится
    /// </summary>
    public int StorageId { get; set; }
    public Storage? Storage { get; set; }

    public List<StoragePlaceMoveDetail>? StoragePlaceMoveDetails { get; set; }
}
