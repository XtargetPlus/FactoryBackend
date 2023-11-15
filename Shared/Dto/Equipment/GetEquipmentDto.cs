namespace Shared.Dto.Equipment;

public class GetEquipmentDto : BaseEquipmentDto
{
    public int SubdivisionId { get; set; }
    public string Subdivision { get; set; } = default!;
}
