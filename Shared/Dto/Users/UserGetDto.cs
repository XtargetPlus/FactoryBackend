namespace Shared.Dto.Users;

public class UserGetDto : BaseUserGetDto
{
    public string Subdivision { get; set; } = default!;
    public string Profession { get; set; } = default!;
}
