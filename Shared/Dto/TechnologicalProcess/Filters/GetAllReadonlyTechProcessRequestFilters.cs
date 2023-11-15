using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.TechnologicalProcess;

public class GetAllReadonlyTechProcessRequestFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// Id типа детали, по умолчанию передавать - С п/п
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int DetailTypeId { get; set; }
    /// <summary>
    /// Id тип заготовки
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int BlankTypeId { get; set; } 
    /// <summary>
    /// Id материала
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int MaterialTypeId { get; set; }
    /// <summary>
    /// Id статуса
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int StatusId { get; set; }
    /// <summary>
    /// Id изделия
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int ProductId { get; set; }
    /// <summary>
    /// По какому полю производится поиск
    /// </summary>
    [DefaultValue(TechProcessReadonlySearchOptions.Base)]
    [Range((int)TechProcessReadonlySearchOptions.Base, (int)TechProcessReadonlySearchOptions.Developer)]
    public TechProcessReadonlySearchOptions SearchOptions { get; set; } 
    /// <summary>
    /// По какому полю производится сортировка
    /// </summary>
    [DefaultValue(TechProcessReadonlyOrderOptions.Base)]
    [Range((int)TechProcessReadonlyOrderOptions.Base, (int)TechProcessReadonlyOrderOptions.Developer)]
    public TechProcessReadonlyOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
