namespace Shared.Dto.Graph.Read.Open;

public class GraphDetailDto
{
    public int GraphDetailId { get; set; }
    public int TechProcessId { get; set; }
    public int LocalPositionNumber { get; set; }
    public string PositionNumberToDisplay { get; set; } = default!;
    public int DetailId { get; set; }
    public string DetailTitle { get; set; } = default!;
    public string DetailSerialNumber { get; set; } = default!;
    public float Usability { get; set; }
    public float? UsabilitySum { get; set; } 
    public float UsabilityWithFathers { get; set; }
    public float PlanCount { get; set; }
    public float? TotalPlanCount { get; set; }
    public float? CountInStream { get; set; }
    public float? FinishedGoodsInventory { get; set; }
    public bool IsHaveOtherTechProcesses { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsDublicate { get; set; }
    public bool IsVisible { get; set; }

    public GraphDetailItemHigherDto ItemsInfo { get; set; }
}