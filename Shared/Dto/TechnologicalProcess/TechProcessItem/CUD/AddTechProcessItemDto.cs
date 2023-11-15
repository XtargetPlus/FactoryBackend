using System.ComponentModel;

namespace Shared.Dto.TechnologicalProcess;

public class AddTechProcessItemDto : TechProcessItemDto
{
    [DefaultValue(5)]
    public int Priority { get; set; }
    public int? MainTechProcessItemId { get; set; }
}
