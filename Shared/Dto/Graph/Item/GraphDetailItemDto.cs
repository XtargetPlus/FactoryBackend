using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Item;

public class GraphDetailItemDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required]
    public int TechProcessItemId { get; set; }
}