using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess.TechProcessItem.Filters;

public class GetAllTechProcessItemsFilters
{
    /// <summary>
    /// Id тех процесса
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TechProcessId { get; set; }
    /// <summary>
    /// Приоритет (ветка) операции
    /// </summary>
    [DefaultValue(5)]
    [Range(5, int.MaxValue)]
    public int Priority { get; set; }
    /// <summary>
    /// Фильтр видимости (отображать скрытые операции или нет)
    /// </summary>
    [DefaultValue(QueryFilterOptions.TurnOn)]
    [Range((int)QueryFilterOptions.TurnOff, (int)QueryFilterOptions.TurnOn)]
    public QueryFilterOptions VisibilityOptions { get; set; }
}
