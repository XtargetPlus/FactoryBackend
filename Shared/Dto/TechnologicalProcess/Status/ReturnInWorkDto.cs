namespace Shared.Dto.TechnologicalProcess.Status;

public class ReturnInWorkDto
{
    public int TechProcessId { get; set; }
    public DateOnly NewFinishDate { get; set; }
    public int DeveloperId { get; set; }
    public byte DeveloperPriority { get; set; }
    public string? Note { get; set; }
}