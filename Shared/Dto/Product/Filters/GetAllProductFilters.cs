using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Product.Filters;

public class GetAllProductFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// По какому полю производится поиск
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.Base)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter ProductSearch { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(ProductOrderOptions.Base)]
    [Range((int)ProductOrderOptions.Base, (int)ProductOrderOptions.ForPrice)]
    public ProductOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
