using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Item;

public class AddToBlockGraphDetailItemDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required] 
    public int Priority { get; set; }

    [Required]
    public int TechProcessItemId { get; set; }
}