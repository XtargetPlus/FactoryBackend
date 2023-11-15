namespace Shared.Dto.TechnologicalProcess.Read;

public class GetDeveloperTasksDto
{
    public int StatusId { get; set; }
    public string Status { get; set; }
    public List<DeveloperTaskDto>? Tasks { get; set; }
}