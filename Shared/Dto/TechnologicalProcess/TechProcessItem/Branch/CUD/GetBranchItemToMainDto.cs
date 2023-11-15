namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;

public class GetBranchItemToMainDto
{
    public int TechProcessId { get; set; }
    public int BeforeMainItemId { get; set; }
    public int CurrentTargetBranchItemId { get; set; }
}
