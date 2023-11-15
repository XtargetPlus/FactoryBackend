using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.TechnologicalProcess;

public class ChangeEquipmentOperationDto : EquipmentOperationDto
{
    [Required]
    public int EquipmentOperationId { get; set; }

    [DefaultValue(0)]
    public int NewEquipmentId { get; set; }
}
