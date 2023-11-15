using DatabaseLayer.Helper;
using Shared.Dto.Detail;
using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;
using Shared.Dto.Tools;

namespace ServiceLayer.Equipments.Services.Interfaces;

public interface IEquipmentService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(AddEquipmentDto dto);
    Task AddDetailAsync(EquipmentWithDetailDto dto);
    Task ChangeAsync(ChangeEquipmentDto dto);
    Task DeleteAsync(int id);
    Task DeleteDetailAsync(EquipmentWithDetailDto dto);
    Task<GetEquipmentDto?> GetFirstAsync(int id);
    Task<List<GetEquipmentDto>?> GetAllAsync(GetAllEquipmentFilters filters);
    Task<List<GetEquipmentDto>?> GetAllBySubdivisionAsync(int subdivisionId);
    //Task<List<GetToolDto>?> GetToolFromEquipmentAsync(int equipmentId);
}
