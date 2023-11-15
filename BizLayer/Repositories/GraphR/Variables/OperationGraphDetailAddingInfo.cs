namespace BizLayer.Repositories.GraphR.Variables;

public class OperationGraphDetailAddingInfo
{
    public float PlannedNumber { get; set; }
    public float? TotalPlannedNumber { get; set; }
    public float Usability { get; set; }
    public float? UsabilitySum { get; set; }
    public int DetailGraphNumberWithoutRepeats { get; set; }
    public string DetailGraphNumberWithRepeats { get; set; } = null!;
    public int DetailId { get; set; }
}