namespace Shared.Dto.Users;

public class UserGetWithSubdivisionDto : BaseUserGetDto
{
    public string Subdivision { get; set; } = default!;
}
