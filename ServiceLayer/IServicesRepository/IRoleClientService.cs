using DatabaseLayer.Helper;
using Shared.Dto.Role;

namespace ServiceLayer.IServicesRepository;

public interface IRoleClientService : IErrorsMapper, IDisposable
{
    Task AddValueAsync(RoleClientDto roleClientDto);
    Task AddRangeAsync(RoleAddRangeWithFuncOnFormsDto rangeRoleDto);
    Task ChangeAsync(RoleClientDto roleClientDto);
    Task ChangeRangeAsync(RoleChangeRangeWithFuncOnFormsDto rangeRoleDto);
    Task DeleteAsync(BaseRoleClientDto roleClientDto);
    Task DeleteRangeAsync(RoleClientDeleteRangeDto rangeRoleClientDto);
    Task<List<RoleClientConcreteDto>?> GetAllAsync();
    Task<List<string>?> GetUserFormsAsync(List<string> forms, string role);
    Task<RoleClientFuncDto?> GetFormFuncAsync(string form, string role);
}
