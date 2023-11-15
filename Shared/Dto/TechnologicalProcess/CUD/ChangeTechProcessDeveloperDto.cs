namespace Shared.Dto.TechnologicalProcess;

public class ChangeTechProcessDeveloperDto
{
    public int TechProcessId { get; set; }
    public int DeveloperId { get; set; }    
    public byte DeveloperPriority { get; set; }
}
