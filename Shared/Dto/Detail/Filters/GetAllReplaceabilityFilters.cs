using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Detail.Filters;

public class GetAllReplaceabilityFilters
{
    /// <summary>
    /// Id детали, чью заменяемость нужно получить
    /// </summary>
    [Range(1, int.MaxValue)]
    public int DetailId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(DetailOrderOptions.Base)]
    [Range((int)DetailOrderOptions.Base, (int)DetailOrderOptions.ForDetailType)]
    public DetailOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
