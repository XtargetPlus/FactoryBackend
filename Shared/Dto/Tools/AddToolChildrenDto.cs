using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddToolChildrenDto
{
    [Required]
    public int FatherId { get; set; }
    [Required]
    public int ChildrenId { get; set; }
    [Required]
    public int Count { get; set; }
    [Required]
    public int Priority { get; set; }
}