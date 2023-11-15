using Shared.Dto.Detail;

namespace Shared.Dto.TechnologicalProcess;

public class GetDetailTechProcessesDto
{
    public BaseIdSerialTitleDto? Detail { get; set; } = default!;
    public List<DetailedTechProcessInfoDto> TechProcesses { get; set; } = new();
}
