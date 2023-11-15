using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Role;

public class RoleClientDeleteRangeDto
{
    [Range(0, int.MaxValue)]
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Необходимо передать id форм"), MinLength(1)]
    public List<int> FormsId { get; set; } = default!;
}
