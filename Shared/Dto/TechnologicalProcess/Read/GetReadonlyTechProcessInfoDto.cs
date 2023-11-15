namespace Shared.Dto.TechnologicalProcess;

public class GetReadonlyTechProcessInfoDto : DetailedTechProcessInfoDto
{
    public string DetailTitle { get; set; } = default!;
    public string DetailSerialNumber { get; set; } = default!;
    public string DetailType { get; set; } = default!;
    public int? MaterialId { get; set; }
    public int? BlankTypeId { get; set; }
}
