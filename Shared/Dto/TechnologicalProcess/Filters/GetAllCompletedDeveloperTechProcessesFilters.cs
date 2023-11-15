using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess.Filters;

public class GetAllCompletedDeveloperTechProcessesFilters : BasePagination
{
    /// <summary>
    /// Id разработчика тех процесса, чьи выполненные тех процессы нужно получить
    /// </summary>
    [Range(1, int.MaxValue)]
    public int DeveloperId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(CompletedDeveloperTechProcessesOrderOptions.Base)]
    [Range((int)CompletedDeveloperTechProcessesOrderOptions.Base, (int)CompletedDeveloperTechProcessesOrderOptions.Title)]
    public CompletedDeveloperTechProcessesOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
