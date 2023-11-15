using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Graph.Status;

public class OperationGraphChangeStatusDto
{
    /// <summary>
    /// Id графика, если это группа, то Id main графика
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int GraphId { get; set; }

    /// <summary>
    /// Id нового статуса
    /// </summary>
    [Required]
    public int StatusId { get; set; }
}