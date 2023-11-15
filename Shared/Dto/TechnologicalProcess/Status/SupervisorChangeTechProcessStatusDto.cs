using Shared.Enums;

namespace Shared.Dto.TechnologicalProcess;

public class SupervisorChangeTechProcessStatusDto : BaseChangeTechProcessStatusDto
{
    public TechProcessStatusesForDirector TechProcessStatuses { get; set; }
}
