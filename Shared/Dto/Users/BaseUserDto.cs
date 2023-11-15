namespace Shared.Dto.Users;

public class BaseUserDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FathersName { get; set; } = default!;
    public string ProfessionNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int ProfessionId { get; set; }
    public int SubdivisionId { get; set; }
    public int StatusId { get; set; }
}
