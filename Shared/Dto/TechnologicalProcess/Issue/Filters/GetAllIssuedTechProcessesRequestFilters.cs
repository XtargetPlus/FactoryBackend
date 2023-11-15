using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.TechnologicalProcess;

public class GetAllIssuedTechProcessesRequestFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// Id типа заготовки
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int BlankTypeId { get; set; }
    /// <summary>
    /// Id материала
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int MaterialId { get; set; }
    /// <summary>
    /// Id разработчика
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int DeveloperId { get; set; }
    /// <summary>
    /// Id изделия
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int ProductId { get; set; }
    /// <summary>
    /// По какому полю производить поиск
    /// </summary>
    [DefaultValue(SerialNumberOrTitleFilter.Base)]
    [Range((int)SerialNumberOrTitleFilter.Base, (int)SerialNumberOrTitleFilter.ForTitle)]
    public SerialNumberOrTitleFilter SearchOptions { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(IssuedTechProcessOrderOptions.Base)]
    [Range((int)IssuedTechProcessOrderOptions.Base, (int)IssuedTechProcessOrderOptions.Developer)]
    public IssuedTechProcessOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
