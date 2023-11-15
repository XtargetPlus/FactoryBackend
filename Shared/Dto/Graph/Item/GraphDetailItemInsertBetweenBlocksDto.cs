namespace Shared.Dto.Graph.Item;

public class GraphDetailItemInsertBetweenBlocksDto
{
    public int GraphDetailId { get; set; }
    public int TargetItemPriority { get; set; }
    public int NewFirstItemNumber { get; set; }
}