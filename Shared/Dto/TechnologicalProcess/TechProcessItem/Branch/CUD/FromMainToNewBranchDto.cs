namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;

public class FromMainToNewBranchDto
{
    public required int MainTechProcessItemId { get; set; }
    public required List<int> MainItemsId { get; set; }
}
