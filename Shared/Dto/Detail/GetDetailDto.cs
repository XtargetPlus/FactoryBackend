namespace Shared.Dto.Detail;

public class GetDetailDto : BaseDto
{
    public string SerialNumber { get; set; } = default!;
    public string DetailType { get; set; } = default!;
    public bool IsComposite { get; set; }
}
