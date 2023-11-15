namespace Shared.Dto.TechnologicalProcess;

public class InsertBetweenItemsDto
{
    public int TechProcessId { get; set; }
    public int BeforeItemId { get; set; }
    public int CurrentTargetItemId { get; set; }
}
