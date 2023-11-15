namespace Shared.Dto.TechnologicalProcess;

public class ChangeTechProcessItemDto
{
    public int TechProcessItemId { get; set; }
    public string OperationNumber { get; set; } = default!;
    public int OperationId { get; set; }
    public int Count { get; set; }
    public string? Note { get; set; }
}
