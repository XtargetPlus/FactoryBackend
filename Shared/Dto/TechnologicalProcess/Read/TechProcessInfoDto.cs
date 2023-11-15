namespace Shared.Dto.TechnologicalProcess;

public class TechProcessInfoDto : BaseTechProcessDataDto
{
    public string Developer { get; set; } = default!;
    public string Status { get; set; } = default!;
}
