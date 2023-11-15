using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Detail;

public class InsertBetweenGraphDetailsDto
{
    [Required]
    public int GraphId { get; set; }

    [Required]
    public int TargetPositionNumber { get; set; }

    [Required]
    public int NewPositionNumber { get; set; }
}