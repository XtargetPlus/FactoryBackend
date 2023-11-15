using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Item;

public class GetAllDetailItemsDto
{
    [Required]
    public int GraphDetailId { get; set; }

    [Required]
    public int TechProcessId { get; set; }
}