using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.CUD;

public class DeleteGraphsInGroupDto
{
    [Required]
    public int MainGraphId { get; set; }

    [Required]
    public List<int> GraphsId { get; set; }

    [Required]
    public DeleteFromGraphsGroupType DeleteType { get; set; }
}