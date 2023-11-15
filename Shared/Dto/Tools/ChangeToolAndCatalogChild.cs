namespace Shared.Dto.Tools;

public class ChangeToolAndCatalogChild
{
    public int FatherId { get; set; }
    public List<int> Catalogs { get; set; } = new List<int>();
    public List<int> Tools { get; set; } = new List<int>();

}
