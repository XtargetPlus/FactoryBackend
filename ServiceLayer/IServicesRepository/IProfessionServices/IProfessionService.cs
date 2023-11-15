using DatabaseLayer.Helper;
using Shared.Dto;
using Shared.Dto.Profession.Filters;

namespace ServiceLayer.IServicesRepository.IProfessionServices;

public interface IProfessionService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(TitleDto dto);
    Task ChangeAsync(BaseDto baseDto);
    Task DeleteAsync(int id);
    Task<List<BaseDto>?> GetAllAsync(GetAllProfessionFilters filters);
    Task<BaseDto?> GetFirstAsync(int id);
}
