using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Users.Filters;

public class GetAllUserFromSubdivisionFilters : BasePagination
{
    /// <summary>
    /// Id подразделения, по которому будет происходить выборка
    /// </summary>
    [Range(0, int.MaxValue)]
    public int SubdivisionId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(UserOrderFromSubdivOptions.Base)]
    [Range((int)UserOrderFromSubdivOptions.Base, (int)UserOrderFromSubdivOptions.ForProf)]
    public UserOrderFromSubdivOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
