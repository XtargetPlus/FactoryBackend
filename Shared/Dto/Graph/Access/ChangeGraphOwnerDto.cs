using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.Access;

public class ChangeGraphOwnerDto
{
    [Required]
    public int OperationGraphId { get; set; }

    [Required]
    public int NewOwnerId { get; set; }

    [Required]
    [DefaultValue(NewUserAccess.Readonly)]
    [Range((int)NewUserAccess.None, (int)NewUserAccess.Readonly)]
    public NewUserAccess NewUserAccess { get; set; }
}