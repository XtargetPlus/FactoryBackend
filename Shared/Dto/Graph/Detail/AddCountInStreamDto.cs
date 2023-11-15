using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Detail;

public class AddCountInStreamDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required]
    [Range(0, float.MaxValue)]
    public float CountInStream { get; set; }
}