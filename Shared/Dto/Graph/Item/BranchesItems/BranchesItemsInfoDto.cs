using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Read.BranchesItems;

public class BranchesItemsInfoDto
{
    [Required]
    public int TechProcessId { get; set; }

    [Required]
    public int Priority { get; set; }
}