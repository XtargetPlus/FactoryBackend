namespace Shared.Dto.Graph.Read.Open;

public class GraphInfoDto
{
    public int GraphId { get; set; }
    public int Priority { get; set; }
    public string GraphDate { get; set; } = default!;
    public string DetailSerialNumber { get; set; } = default!;
    public string DetailTitle { get; set; } = default!;
    public float PlanCount { get; set; }
    public int SubdivisionId { get; set; }
    public string Subdivision { get; set; } = default!;
    public int CurrentStatusId { get; set; }
    public string CurrentStatusTitle { get; set; } = default!;
    public bool ReadOnly { get; set; } = default!;
    public string? Note { get; set; }
    public bool IsConfirmed { get; set; }

    public List<GraphPossibleStatusDto> PossibleStatuses { get; set; }
    public List<GraphDetailDto> GraphDetails { get; set; }
}