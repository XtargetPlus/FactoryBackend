using Shared.Dto.Graph.Read.Open;

namespace Shared.Dto.Graph.Read;

public class OpenOperationGraphDto
{
    public bool IsStatusConfirmed { get; set; }

    public GraphInfoDto GraphInfo { get; set; }
}