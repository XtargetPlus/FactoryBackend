using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddToolEquipmentDto
{
    [Range(1,int.MaxValue)]
    public int ToolId { get; set; }
    [Range(1, int.MaxValue)]
    public int EquipmentId { get; set; }
}