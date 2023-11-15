using Shared.Dto.Graph.Read.Open;

namespace Shared.Dto.Graph.Read;

public class OpenOperationGraphGroupDto
{
    public int MainGraphId { get; set; }
    public bool IsGroupConfirmed { get; set; }
    public bool IsStatusConfirmed { get; set; }

    public List<GraphInfoDto> Graphs { get; set; }
}