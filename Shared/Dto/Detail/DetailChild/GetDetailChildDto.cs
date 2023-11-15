namespace Shared.Dto.Detail.DetailChild;

public class GetDetailChildDto : BaseDto
{
    public string SerialNumber { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public float Count { get; set; }
    public int Number { get; set; }
    public bool IsComposite { get; set; }
}
