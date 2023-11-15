using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddToolCatalogDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int FatherId { get; set; }
}