namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;

public class FromMainToBranchDto : BaseBranchDto
{
    public int TechProcessId { get; set; }
    public int BeforeBranchItemId { get; set; }
    public int CurrentTargetMainItemId { get; set; }
}
