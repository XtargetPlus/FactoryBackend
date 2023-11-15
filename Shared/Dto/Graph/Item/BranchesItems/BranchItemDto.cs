namespace Shared.Dto.Graph.Read.BranchesItems;

public class BranchItemDto
{
    public int TechProcessItemId { get; set; }
    public string OperationNumber { get; set; } = default!;
    public string ShortName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public int Priority { get; set; } = default!;
    public int Count { get; set; }
    public string? Note { get; set; }
}