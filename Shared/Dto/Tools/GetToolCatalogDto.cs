namespace Shared.Dto.Tools;

public class GetToolCatalogDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Key { get; set; }
    public bool HasChild { get; set; }
}