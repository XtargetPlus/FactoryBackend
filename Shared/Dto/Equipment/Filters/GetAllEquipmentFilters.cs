using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Equipment.Filters;

public class GetAllEquipmentFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// Id подразделения
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int SubdivisionId { get; set; }
    /// <summary>
    /// По какому полю производится поиска
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.ForSerialNumber)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter SearchOptions { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.ForSerialNumber)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Up)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
