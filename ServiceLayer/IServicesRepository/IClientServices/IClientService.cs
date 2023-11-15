using DatabaseLayer.Helper;
using Shared.Dto;

namespace ServiceLayer.IServicesRepository.IClientServices;

public interface IClientService : IErrorsMapper
{
    Task<int?> AddAsync(TitleDto dto);
    Task ChangeAsync(BaseDto baseRequest);
    Task DeleteAsync(int id);
    Task<List<BaseDto>?> GetAllAsync(int take = 50, int skip = 0);
    Task<BaseDto?> GetFirstAsync(int id);
}
