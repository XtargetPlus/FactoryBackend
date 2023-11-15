using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.Access;

public class ChangeUserAccessDto
{
    [Required]
    public int GraphId { get; set; }
    
    [Required]
    public int UserId { get; set; }

    [Required]
    [Range((int)NewUserAccess.ReadAndEdit, (int)NewUserAccess.Readonly)]
    public NewUserAccess NewAccess { get; set; }
}