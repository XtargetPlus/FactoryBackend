namespace Shared.Dto.Graph.Item;

public class InsertBetweenGraphDetailItemInBlockDto
{
    public int GraphDetailId { get; set; }
    public int TargetItemNumber { get; set; }
    public int NewItemNumber { get; set; }
}