namespace Shared.Dto.TechnologicalProcess;

public class ChangeTechProcessData
{
    public int TechProcessId { get; set; }
    public int BlankTypeId { get; set; } 
    public int MaterialId { get; set; } 
    public string Rate { get; set; } = default!;
}
