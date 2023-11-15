namespace Shared.Dto.TechnologicalProcess.Read;

public class DeveloperTaskDto
{
    public int TechProcessId { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateOnly Date { get; set; }
    public byte Priority { get; set; }
    public int StatusId { get; set; }
    public string? Note { get; set; }
}