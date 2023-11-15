using DB.Model.SubdivisionInfo;

namespace DB.Model.StorageInfo;

/// <summary>
/// Склад
/// </summary>
public class Storage : BaseModel
{
    /// <summary>
    /// Название склада
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Физический или не физический склад
    /// </summary>
    public bool IsPhysicalStorage { get; set; }
    /// <summary>
    /// К какому складу относится
    /// </summary>
    public int SubdivisionId { get; set; }
    public Subdivision? Subdivision { get; set; }
    /// <summary>
    /// Родительская группа складов
    /// </summary>
    public int? FatherStorageId { get; set; }
    public Storage? FatherStorage { get; set; }

    public List<Storage>? ChildrenStorages { get; set; }
    public List<MoveDetail>? MoveDetails { get; set; }
    public List<StoragePlace>? StoragePlaces { get; set; }
} 
