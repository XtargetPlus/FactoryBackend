namespace Shared.Dto.Role;

public class RoleClientConcreteDto : BaseRoleClientDto
{
    public string Role { get; set; } = default!;
    public string UserForm { get; set; } = default!;
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Browsing { get; set; }
}
