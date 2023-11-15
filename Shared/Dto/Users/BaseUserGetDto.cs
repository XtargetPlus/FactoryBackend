namespace Shared.Dto.Users;

public class BaseUserGetDto
{
    public int Id { get; set; }
    public string ProfessionNumber { get; set; } = default!;
    public string FFL { get; set; } = default!;
}
