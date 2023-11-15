using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.Filters;

public class OpenOperationGraphFilters
{
    /// <summary>
    /// Id операционного графика
    /// </summary>
    [Required]
    public int GraphId { get; set; }

    /// <summary>
    /// Тип открытия: 0 - с повторами деталей, 1 - без повторов деталей
    /// </summary>
    [DefaultValue(GraphOpenType.WithRepeats)]
    [Range((int)GraphOpenType.WithRepeats, (int)GraphOpenType.WithoutRepeats)]
    public GraphOpenType GraphOpenType { get; set; }

    /// <summary>
    /// Фильтр видимости деталей: 0 - все (в работе и выполненные), 1 - только те, что в работе
    /// </summary>
    [DefaultValue(GraphDetailVisibility.All)]
    [Range((int)GraphDetailVisibility.All, (int)GraphDetailVisibility.InWork)]
    public GraphDetailVisibility DetailVisibility { get; set; }
}