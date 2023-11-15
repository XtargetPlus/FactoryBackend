namespace Shared.Dto.TechnologicalProcess;

public class DetailedTechProcessInfoDto : TechProcessInfoDto
{
    public int TechProcessId { get; set; }
    public int ManufacturingPriority { get; set; }
    public string FinishDate { get; set; } = default!;
    public string StatusDate { get; set; } = default!;
}
