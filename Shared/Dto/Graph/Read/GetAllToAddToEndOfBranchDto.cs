using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Read;

public class GetAllToAddToEndOfBranchDto
{
    /// <summary>
    /// Id детали графика
    /// </summary>
    [Required]
    public int GraphDetailId { get; set; }

    /// <summary>
    /// Id тех процесса детали графика
    /// </summary>
    [Required]
    public int TechProcessId { get; set; }

    /// <summary>
    /// Приоритет блока операций детали графика, для которого нужно получить список операций тех процесса, которые можно добавить в этот блок
    /// </summary>
    [Required]
    public int Priority { get; set; }
}