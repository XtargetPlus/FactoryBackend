using DatabaseLayer.Helper;
using Shared.Dto;

namespace ServiceLayer.IServicesRepository.IOutsideOrganizationCountServices;

public interface IOutsideOrganizationService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(TitleDto dto);
    Task ChangeAsync(BaseDto baseRequest);
    Task DeleteAsync(int id);
    Task<List<BaseDto>?> GetAllAsync(int take = 50, int skip = 0);
    Task<BaseDto?> GetFirstAsync(int id);
}
