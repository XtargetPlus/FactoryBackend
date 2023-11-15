using DatabaseLayer.Helper;
using Shared.Dto.Role;
using Shared.Dto.Users;
using Shared.Dto.Users.Filters;

namespace ServiceLayer.IServicesRepository.IUserServices;

public interface IUserService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(BaseUserDto value);
    Task<string?> ChangeRoleWithPasswordAsync(ChangeUserRoleDto userRole);
    Task ChangeAsync(UserChangeDto value);
    Task<List<UserGetDto>?> GetAllAsync(GetAllUserFilters filters);
    Task DeleteAsync(int id);
    Task<UserGetForHubDto?> GetFirstAsync(int id);
    Task<List<UserGetWithSubdivisionDto>?> GetAllFromProfessionAsync(GetAllUserFromProfessionFilters filters);
    Task<List<UserGetProfessionDto>?> GetAllFromSubdivisionAsync(GetAllUserFromSubdivisionFilters filters);
    Task<List<BaseUserGetDto>?> GetAllTechnologistsDevelopersAsync();
    Task<UserListInfoDto?> GetMoreInfoAsync(int id);
    Task<UserAddInfoDto> GetAddInfoDtoAsync();
    Task<UserRoleDto?> GetUserRoleAsync(int userId);
}
