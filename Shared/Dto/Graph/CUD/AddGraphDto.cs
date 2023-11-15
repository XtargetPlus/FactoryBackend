namespace Shared.Dto.Graph.CUD;

public record AddGraphDto(
    int SubdivisionId, 
    DateOnly GraphDate, 
    int? DetailId, 
    float PlanCount, 
    string? Note
);