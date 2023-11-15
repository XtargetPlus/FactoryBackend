using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Users.Filters;

public class GetAllUserFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// Id статуса для выборки
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int StatusId { get; set; }
    /// <summary>
    /// По какому полю выполняется поиск
    /// </summary>
    [DefaultValue(UserSearchOptions.Base)]
    [Range((int)UserSearchOptions.Base, (int)UserSearchOptions.ForProfNumber)]
    public UserSearchOptions SearchOptions { get; set; }
    /// <summary>
    /// По какому полю выполняется сортировка
    /// </summary>
    [DefaultValue(UserOrderOptions.Base)]
    [Range((int)UserOrderOptions.Base, (int)UserOrderOptions.ForProfNumber)]
    public UserOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
