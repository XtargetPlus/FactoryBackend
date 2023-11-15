using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class ChangeToolDto
{
    [Required]
    public int CatalogId { get; set; }
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Serial { get; set; }
    public string Note { get; set; }
}