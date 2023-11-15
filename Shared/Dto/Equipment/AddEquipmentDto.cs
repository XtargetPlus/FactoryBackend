namespace Shared.Dto.Equipment;

public class AddEquipmentDto
{
    public string Title { get; set; } = default!;
    public string SerialNumber { get; set; } = default!;
    public int SubdivisionId { get; set; }
}
