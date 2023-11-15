using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.Access;

public class AddGraphsInGroupDto
{
    [Required]
    public int MainGraphId { get; set; }

    [Required]
    public List<int> GraphsId { get; set; }
}