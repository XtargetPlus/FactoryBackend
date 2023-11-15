using DatabaseLayer.Helper;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolEquipment.Filters;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolEquipmentService : IErrorsMapper, IDisposable
{
    Task AddEquipmentAsync(AddToolEquipmentDto dto);
    Task ChangeEquipmentAsync(AddToolEquipmentDto dto);
    Task DeleteEquipmentAsync(AddToolEquipmentDto dto);
    Task<List<GetToolEquipmentDto>?> GetEquipmentAsync(GetAllEquipmentFilters filters);
}