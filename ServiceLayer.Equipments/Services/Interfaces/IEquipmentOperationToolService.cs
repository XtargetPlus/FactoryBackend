using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;

namespace ServiceLayer.Equipments.Services.Interfaces;

public interface IEquipmentOperationToolService : IDisposable
{
    Task AddEquipmentOperationTool(AddEquipmentOperationTool dto);
    Task<GetAllEquipmentOperationToolsDto> GetEquipmentOperationTool(GetAllEquipmentOperationToolsFilters dto);
}
