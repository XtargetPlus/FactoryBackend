namespace Shared.Dto.Graph.Read.Open;

public class GraphPossibleStatusDto
{
    public int StatusId { get; set; }
    public string StatusTitle { get; set; } = default!;
    public bool NeedSupervisorConfirmation { get; set; }
}