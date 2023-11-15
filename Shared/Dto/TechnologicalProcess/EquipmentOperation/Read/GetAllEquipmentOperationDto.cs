using Shared.Dto.Graph.Read.BranchesItems;

namespace Shared.Dto.TechnologicalProcess.EquipmentOperation.Read;

public class GetAllEquipmentOperationDto
{
    public BranchItemDto? TechProcessInfo { get; set; }
    public List<GetEquipmentOperationDto>? EquipmentOperations { get; set; }
}