namespace Shared.Dto.TechnologicalProcess;

public class InsertBetweenBranchDto
{
    public int MainTechProcessItemId { get; set; }
    public int BeforeBranch { get; set; }
    public int CurrentTargetBranch { get; set; }
}
