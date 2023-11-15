using Shared.Enums;
using Shared.Static;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess;

public class IssuedTechProcessesFromTechnologistRequestFilters : BasePagination
{
    /// <summary>
    /// По какому полю сортировать
    /// </summary>
    [DefaultValue(IssuedTechProcessesFromTechnologistOrderOptions.DateOfIssue)]
    [Range((int)IssuedTechProcessesFromTechnologistOrderOptions.Base, (int)IssuedTechProcessesFromTechnologistOrderOptions.Subdivision)]
    public IssuedTechProcessesFromTechnologistOrderOptions OrderOptions { get; set; }
    /// <summary>
    /// В какую сторону сортировать
    /// </summary>
    [DefaultValue(KindOfOrder.Down)]
    [Range((int)KindOfOrder.Base, (int)KindOfOrder.Down)]
    public KindOfOrder KindOfOrder { get; set; }
}
