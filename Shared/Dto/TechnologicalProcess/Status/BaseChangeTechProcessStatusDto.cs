namespace Shared.Dto.TechnologicalProcess;

public class BaseChangeTechProcessStatusDto
{
    public int TechProcessId { get; set; }
    public string? Note { get; set; }
    public DateTime Added { get; } = DateTime.Now;
}
