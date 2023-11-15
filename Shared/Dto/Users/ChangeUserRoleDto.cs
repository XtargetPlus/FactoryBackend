namespace Shared.Dto.Users;

public class ChangeUserRoleDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string Password { get; set; } = default!;
}
