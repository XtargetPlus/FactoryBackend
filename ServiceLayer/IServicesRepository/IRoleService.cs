using DatabaseLayer.Helper;
using Shared.Dto;
using Shared.Dto.Role;

namespace ServiceLayer.IServicesRepository;

public interface IRoleService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(TitleDto dto);
    Task ChangeAsync(BaseDto baseRequest);
    Task DeleteAsync(int id);
    Task<List<BaseDto>?> GetAllAsync(string text = "");
    Task<List<string>?> GetAllTitlesAsync();
}
