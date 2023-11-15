namespace Shared.Dto.Graph.Read;

public class GetAllSinglesOperationGraphDto
{
    public int OperationGraphId { get; set; }
    public int Priority { get; set; }
    public string GraphDate { get; set; } = default!;
    public string DetailSerialNumber { get; set; } = default!;
    public string DetailTitle { get; set; } = default!;
    public float PlanCount { get; set; }
    public string Subdivision { get; set; } = default!;
    public string? Note { get; set; }
}