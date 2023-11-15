namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Read;

public class GetAllDevelopersTasksDto
{
    public int DeveloperId { get; set; }
    public string Developer { get; set; } = default!;

    public List<GetExtendedTechProcessDataDto> Tasks { get; set; } = null!;
}