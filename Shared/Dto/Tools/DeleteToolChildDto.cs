using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class DeleteToolChildDto
{
    [Range(1, int.MaxValue)]
    public int FatherId { get; set; }
    [Range(1, int.MaxValue)] 
    public int ChildId { get; set; }
}