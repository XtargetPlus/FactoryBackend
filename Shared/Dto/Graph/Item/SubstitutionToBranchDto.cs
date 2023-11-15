using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Read;

public class SubstitutionToBranchDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required]
    public int OldDetailItemPriority { get; set; }

    [Required]
    public int NewDetailItemPriority { get; set;  }
}