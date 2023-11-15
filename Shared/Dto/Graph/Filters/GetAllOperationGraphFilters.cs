using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Graph.Filters;

public class GetAllOperationGraphFilters
{
    /// <summary>
    /// Дата начало в фильтре по дате
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// Дата конце в фильтре по дате
    /// </summary>
    public DateOnly? EndDate { get; set;}

    /// <summary>
    /// Какому подразделению пренадлежит график
    /// </summary>
    [Range(0, int.MaxValue)]
    public int SubdivisionId { get; set; }

    /// <summary>
    /// Статус графика
    /// </summary>
    [DefaultValue(0)]
    public GraphStatus Status { get; set; }

    /// <summary>
    /// Тип владения графика: 0 - все, 1 - только владельца, 2 - только с предоставленным доступом
    /// </summary>
    [DefaultValue(GraphOwnershipType.All)]
    [Range((int)GraphOwnershipType.All, (int)GraphOwnershipType.AccessProvided)]
    public GraphOwnershipType OwnershipType { get; set; }

    /// <summary>
    /// Тип доступа графику: 0 - все, 1 - только с полным, 2 - только на чтение
    /// </summary>
    [DefaultValue(GraphAccessTypeForFilters.All)]
    [Range((int)GraphAccessTypeForFilters.All, (int)GraphAccessTypeForFilters.Readonly)]
    public GraphAccessTypeForFilters AccessTypeForFilters { get; set; }

    /// <summary>
    /// Наличие у графика изделия: 0 - все, 1 - только с изделиями, 2 - только без изделия
    /// </summary>
    [DefaultValue(GraphProductAvailability.All)]
    [Range((int)GraphProductAvailability.All, (int)GraphProductAvailability.HaveNot)]
    public GraphProductAvailability ProductAvailability { get; set; }

    /// <summary>
    /// В какую сторону сортировать по приоритету
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}