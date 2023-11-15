namespace Shared.Dto.Graph.CUD;

public record ChangeGraphDto(
    int OperationGraphId,
    float PlanCount,
    DateOnly GraphDate,
    int SubdivisionId,
    string? Note
);