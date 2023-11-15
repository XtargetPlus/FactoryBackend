using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Access;

public class GiveAccessGraphDto
{
    [Required]
    public int OperationGraphId { get; set; }

    [Required] 
    public Dictionary<int, bool> UserAccesses { get; set; } = null!;
}