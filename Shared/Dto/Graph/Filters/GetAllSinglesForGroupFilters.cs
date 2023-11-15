using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Graph.Filters;

public class GetAllSinglesForGroupFilters
{
    /// <summary>
    /// Налачие изделия в графике: 0 - все, 1 - только с изделиями, 2 - только без изделия
    /// </summary>
    [DefaultValue(GraphProductAvailability.All)]
    [Range((int)GraphProductAvailability.All, (int)GraphProductAvailability.HaveNot)]
    public GraphProductAvailability ProductAvailability { get; set; }

    /// <summary>
    /// Какие графики нужно игнорировать
    /// </summary>
    [MaybeNull]
    public List<int> IgnoredGraphIds { get; set; }
}