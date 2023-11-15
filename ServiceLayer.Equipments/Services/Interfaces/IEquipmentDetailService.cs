using DatabaseLayer.Helper;
using Shared.Dto.Detail;
using Shared.Dto.Equipment.Filters;

namespace ServiceLayer.Equipments.Services.Interfaces;

public interface IEquipmentDetailService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(BaseSerialTitleDto equipmentDetailDto);
    Task ChangeAsync(BaseIdSerialTitleDto equipmentDetailDto);
    Task DeleteAsync(int id);
    Task<List<BaseIdSerialTitleDto>?> GetAllAsync(GetAllEquipmentDetailFilters filters);
    Task<List<BaseIdSerialTitleDto>?> GetAllFromEquipmentAsync(GetAllEquipmentDetailsFromEquipmentFilters filters);
}
