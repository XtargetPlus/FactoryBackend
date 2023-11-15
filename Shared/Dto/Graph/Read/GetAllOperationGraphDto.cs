namespace Shared.Dto.Graph.Read;

public class GetAllOperationGraphDto
{
    public int OperationGraphId { get; set; }
    public int Priority { get; set; }
    public string GraphDate { get; set; } = default!;
    public string DetailSerialNumber { get; set; } = default!;
    public string DetailTitle { get; set; } = default!;
    public float PlanCount { get; set; }
    public int SubdivisionId { get; set; }
    public string Subdivision { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Access { get; set; } = default!;
    public string? Note { get; set; }
    public bool IsMainGraph { get; set; }
}