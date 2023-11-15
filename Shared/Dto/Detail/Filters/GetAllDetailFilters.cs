using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Detail.Filters;

public class GetAllDetailFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// Id типа детали для выборки
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int DetailTypeId { get; set; }
    /// <summary>
    /// Id изделия для выборки
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int ProductId { get; set; }
    /// <summary>
    /// Составная деталь или нет
    /// </summary>
    [DefaultValue(IsCompoundDetailOptions.Base)]
    [Range((int)IsCompoundDetailOptions.Base, (int)IsCompoundDetailOptions.NonCompound)]
    public IsCompoundDetailOptions CompoundDetailOptions { get; set; }
    /// <summary>
    /// По какому полю искать
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.Base)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter SearchOptions { get; set; }
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
