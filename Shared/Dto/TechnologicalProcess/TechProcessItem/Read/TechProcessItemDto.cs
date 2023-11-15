namespace Shared.Dto.TechnologicalProcess;

public class TechProcessItemDto
{
    public string OperationNumber { get; set; } = default!;
    public int OperationId { get; set; }
    public int TechProcessId { get; set; }
    public int Count { get; set; }
    public string? Note { get; set; }
}
