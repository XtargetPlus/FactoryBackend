namespace Shared.Dto.Graph.Read;

public class GetAllOperationGraphDictionaryDto<TDto>
{
    public int Priority { get; set; }
    public int MainGraphId { get; set; }
    public bool IsGroup { get; set; }
    public List<TDto> Graphs { get; set; }
}