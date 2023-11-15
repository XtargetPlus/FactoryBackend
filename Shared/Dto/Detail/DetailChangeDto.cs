namespace Shared.Dto.Detail;

public class DetailChangeDto : BaseDto
{
    public string SerialNumber { get; set; } = default!;
    public float Weight { get; set; }
    public int DetailTypeId { get; set; }
    public int UnitId { get; set; }
}
