using Shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess.Issue.Filters;

public class GetAllIssuedDuplicatesTechProcessFilters
{
    /// <summary>
    /// Id тех процесса, чьи дубликаты нужно получить
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TechProcessId { get; set; }
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(IssuedDuplicateTechProcessOrderOptions.DateOfIssue)]
    [Range((int)IssuedDuplicateTechProcessOrderOptions.Base, (int)IssuedDuplicateTechProcessOrderOptions.Subdivision)]
    public IssuedDuplicateTechProcessOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Down)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
