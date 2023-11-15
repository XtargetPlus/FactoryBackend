namespace Shared.Dto.TechnologicalProcess;

public class GetExtendedTechProcessDataDto : GetBaseTechProcessDto
{
    public int Priority { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = default!;
}
