namespace Shared.Dto.Graph.Read.BranchesItems;

public class GetAllBranchesItemsDto
{
    public int Priority { get; set; }
    public bool IsMainBranch { get; set; } 
    public List<GetAllToAddToEndDto> Items { get; set; }
}