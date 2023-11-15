namespace DB.Model;

/// <summary>
/// Базовая модель с наименованием
/// </summary>
public class BaseTitleModel 
{
    public int Id { get; set; }
    /// <summary>
    /// Наименование
    /// </summary>
    public string Title { get; set; } = null!;
}
