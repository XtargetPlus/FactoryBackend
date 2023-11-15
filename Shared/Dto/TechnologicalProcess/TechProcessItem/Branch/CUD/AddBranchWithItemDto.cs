namespace Shared.Dto.TechnologicalProcess;

public class AddBranchWithItemDto
{
    public required TechProcessItemDto NewItemInformation { get; set; } 
    public required int MainTechProcessItemId { get; set; }
}
