using DatabaseLayer.Helper;
using Shared.Dto;
using Shared.Dto.Subdiv;
using System.ComponentModel;

namespace ServiceLayer.IServicesRepository;

public interface ISubdivisionService : IErrorsMapper, IDisposable
{
    Task<int?> AddValueAsync(BaseSubdivisionDto subdivisionRequest);
    Task ChangeAsync(BaseDto subdivisionRequest);
    Task DeleteAsync(int id);
    Task<List<SubdivisionGetDto>?> GetAllLevelAsync([DefaultValue(null)] int? fatherId);
    Task<List<BaseDto>?> GetAllAsync();
    Task<List<BaseDto>?> GetAllByEquipmentContainAsync(bool isContainEquipments = false);
    Task<SubdivisionGetDto?> GetFirstAsync(int id);
    Task<List<BaseDto>?> GetAllWithoutTechProcessAsync(int techProcessId);
}
