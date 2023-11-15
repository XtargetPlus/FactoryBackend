namespace Shared.Dto.TechnologicalProcess;

public class GetTechProcessDuplicateDto
{
    public int TechProcessStatusId { get; set; }
    public int UserId { get; set; }
    public string Issued { get; set; } = default!;
    public string FFL { get; set; } = default!;
    public string ProfessionNumber { get; set; } = default!;
    public int SubdivisionId { get; set; } = default!;
    public string Subdivision { get; set; } = default!;
}
