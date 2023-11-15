namespace Shared.Dto.Users;

public class UserDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FathersName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ProfessionNumber { get; set; } = default!;
    public int ProfessionId { get; set; }
    public string Profession { get; set; } = default!;
    public int SubdivisionId { get; set; }
    public string Subdivision { get; set; } = default!;
    public int StatusId { get; set; }
    public string Status { get; set; } = default!;
}
