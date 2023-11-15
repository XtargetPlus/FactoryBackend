using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Equipment.Filters;

/// <summary>
/// Фильтры выборки деталей станка в форме станков
/// </summary>
public class GetAllEquipmentDetailsFromEquipmentFilters
{
    /// <summary>
    /// Детали какого станка выводить
    /// </summary>
    [Range(1, int.MaxValue)]
    public int EquipmentId { get; set; }
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
