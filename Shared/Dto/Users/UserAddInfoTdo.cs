namespace Shared.Dto.Users;

public class UserAddInfoDto
{
    public List<BaseDto>? Subdivisions { get; set; } = new();
    public List<BaseDto>? Statuses { get; set; }
    public List<BaseDto>? Professions { get; set; }
}
