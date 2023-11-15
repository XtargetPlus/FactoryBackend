namespace Shared.Dto.Tools;

public class GetToolChildDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Serial { get; set; }
    public string? Note { get; set; }
    public int Count { get; set; }
    public int Priority { get; set; }
}