using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess;

public class DeleteTechProcessDuplicateDto
{
    /// <summary>
    /// Id тех процесса
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TechProcessId { get; set; }
    /// <summary>
    /// Пользователь, который получил дубликат
    /// </summary>
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    /// <summary>
    /// Подразделение, в которое был выдан тех процесс
    /// </summary>
    [Range(1, int.MaxValue)]
    public int SubdivisionId { get; set; }
}
