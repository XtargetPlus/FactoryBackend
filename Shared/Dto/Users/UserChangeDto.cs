namespace Shared.Dto.Users;

public class UserChangeDto 
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FathersName { get; set; } = default!;
    public string ProfessionNumber { get; set; } = default!;
    public int ProfessionId { get; set; }
    public int SubdivisionId { get; set; }
    public int StatusId { get; set; }
}
