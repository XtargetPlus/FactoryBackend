namespace Shared.Dto.Product;

public class ProductGetDto
{
    public int ProductId { get; set; }
    public float Price { get; set; }
    public int DetailId { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
}
