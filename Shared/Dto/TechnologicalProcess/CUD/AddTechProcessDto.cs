namespace Shared.Dto.TechnologicalProcess;

public class AddTechProcessDto
{
    public int DetailId { get; set; }
    public DateOnly FinishDate { get; set; }
    public byte DevelopmentPriority { get; set; }
    public int DeveloperId { get; set; }
    public string? Note { get; set; } 
}
