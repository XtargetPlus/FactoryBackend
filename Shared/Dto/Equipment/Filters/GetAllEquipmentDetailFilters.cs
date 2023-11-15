using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Equipment.Filters;

public class GetAllEquipmentDetailFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [DefaultValue("")]
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// По какому полю искать
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.Base)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter SearchOptions { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.Base)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
