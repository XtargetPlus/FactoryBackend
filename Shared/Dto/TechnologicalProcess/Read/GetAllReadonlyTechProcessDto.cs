using Shared.Dto.Detail;

namespace Shared.Dto.TechnologicalProcess;

public class GetAllReadonlyTechProcessDto : BaseIdSerialTitleDto
{
    public int Id { get; set; }
    public string BlankType { get; set; } = default!;
    public string Material { get; set; } = default!;
    public string Rate { get; set; } = default!;
    public string Developer { get; set; } = default!;
    public string Status { get; set; } = default!;
}
