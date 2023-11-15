using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Operation.Filters;

public class GetAllOperationFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
    /// <summary>
    /// По какому полю производится поиск
    /// </summary>
    [DefaultValue(OperationFilterOptions.Base)]
    [Range((int)OperationFilterOptions.Base, (int)OperationFilterOptions.ForFullName)]
    public OperationFilterOptions SearchOptions { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(OperationFilterOptions.Base)]
    [Range((int)OperationFilterOptions.Base, (int)OperationFilterOptions.ForFullName)]
    public OperationFilterOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Base)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }

}
