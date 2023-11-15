using Shared.Enums;

namespace Shared.Dto.TechnologicalProcess;

public class DeveloperChangeTechProcessStatusDto : BaseChangeTechProcessStatusDto
{
    public TechProcessStatusesForDeveloper TechProcessStatuses { get; set; }
}
