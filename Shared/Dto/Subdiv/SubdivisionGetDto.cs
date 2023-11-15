namespace Shared.Dto.Subdiv;

public class SubdivisionGetDto : BaseDto
{
    public int? FatherId { get; set; } = null!;
    public int CountChildren { get; set; }
}
