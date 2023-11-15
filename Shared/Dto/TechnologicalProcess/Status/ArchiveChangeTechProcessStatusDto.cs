using Shared.Enums;

namespace Shared.Dto.TechnologicalProcess;

public class ArchiveChangeTechProcessStatusDto : BaseChangeTechProcessStatusDto
{
    public TechProcessStatusesForArchive TechProcessStatusId { get; set; }
}
