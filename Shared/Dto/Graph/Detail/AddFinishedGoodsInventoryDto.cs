using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Detail;

public class AddFinishedGoodsInventoryDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required]
    [Range(0, float.MaxValue)]
    public float FinishedGoodsInventory { get; set; }
}