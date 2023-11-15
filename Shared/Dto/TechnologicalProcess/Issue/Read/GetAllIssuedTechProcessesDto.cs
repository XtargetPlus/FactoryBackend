namespace Shared.Dto.TechnologicalProcess;

public class GetAllIssuedTechProcessesDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Material { get; set; } = default!;
    public string BlankType { get; set; } = default!;
    public string Developer { get; set; } = default!;
}
