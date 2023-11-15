using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Read;

public class AddToEndOfMainInfoDto
{
    /// <summary>
    /// Id детали графика
    /// </summary>
    [Required]
    public int GraphDetailId { get; set; }

    /// <summary>
    /// Id тех процесса этой детали
    /// </summary>
    [Required]
    public int TechProcessId { get; set; }
}