namespace Shared.Dto.Detail.DetailChild;

/// <summary>
/// 
/// </summary>
public class GetProductDetailsWithUsabilityDto
{
    public int DetailId { get; set; }
    public string PositionNumber { get; set; } = default!;
    public float Usability { get; set; } 
}