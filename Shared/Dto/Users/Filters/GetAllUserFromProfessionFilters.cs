using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Shared.Static;

namespace Shared.Dto.Users.Filters;

public class GetAllUserFromProfessionFilters : BasePagination
{
    /// <summary>
    /// Id подразделения, по которому будет происходить выборка
    /// </summary>
    [Range(0, int.MaxValue)]
    public int ProfessionId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(UserOrderFromProfOptions.Base)]
    [Range((int)UserOrderFromProfOptions.Base, (int)UserOrderFromProfOptions.ForSubdiv)]
    public UserOrderFromProfOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
