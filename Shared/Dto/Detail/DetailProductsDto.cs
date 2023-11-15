namespace Shared.Dto.Detail;

public class DetailProductsDto
{
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public float Count { get; set; }
}
