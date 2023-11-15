namespace Shared.Dto.TechnologicalProcess;

public class GetTechProcessItemDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Priority { get; set; }
    public string OperationNumber { get; set; } = default!;
    public int OperationId { get; set; }
    public string ShortName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public int Count { get; set; }
    public string? Note { get; set; }
    public bool Show { get; set; } = true;
}
