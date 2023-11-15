using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Detail.DetailChild.Filters;

public class GetAllChildrenFilters
{
    /// <summary>
    /// Id детали родителя
    /// </summary>
    [Range(1, int.MaxValue)]
    public int FatherId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(DetailChildOrderOptions.ForNumber)]
    [Range((int)DetailChildOrderOptions.Base, (int)DetailChildOrderOptions.ForTitle)]
    public DetailChildOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
