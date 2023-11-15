using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Equipment.Filters;

public class GetAllEquipmentOperationToolsFilters
{
    [Required]
    [Range(1, int.MaxValue)]
    public int EquipmentOperationId { get; set; }
}
