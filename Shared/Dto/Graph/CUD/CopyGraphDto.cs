namespace Shared.Dto.Graph.CUD;

public class CopyGraphDto
{
    public int GraphId { get; set; }
    public DateOnly GraphDate { get; set; }
    public float PlanCount { get; set; }
    public string? Note { get; set; }
    public int SubdivisionId { get; set; }
}