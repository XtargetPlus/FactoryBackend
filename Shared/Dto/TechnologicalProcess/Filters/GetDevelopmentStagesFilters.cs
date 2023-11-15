using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess.Filters;

public class GetDevelopmentStagesFilters
{
    /// <summary>
    /// Id тех процесса, чьи этапы разработки нужно получить
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TechProcessId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(TechProcessDevelopmentStagesOrderOptions.StatusDate)]
    [Range((int)TechProcessDevelopmentStagesOrderOptions.Base, (int)TechProcessDevelopmentStagesOrderOptions.Status)]
    public TechProcessDevelopmentStagesOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
