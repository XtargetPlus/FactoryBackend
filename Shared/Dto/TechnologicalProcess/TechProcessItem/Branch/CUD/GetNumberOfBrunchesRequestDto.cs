using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;

public class GetNumberOfBrunchesRequestDto
{
    /// <summary>
    /// Id операции тех процесса, чьи ветки нужно получить
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int TechProcessItemId { get; set; }

    /// <summary>
    /// true - только видимые, false - все (скрытые и не скрытые)
    /// </summary>
    [DefaultValue(true)]
    public bool Visibility { get; set; }
}