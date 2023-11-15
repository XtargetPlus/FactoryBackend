using DatabaseLayer.Helper;
using Shared.Dto.Graph.Access;
using Shared.Dto.Users;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphAccessService : IErrorsMapper, IDisposable
{
    Task ChangeOwnerAsync(ChangeGraphOwnerDto dto);
    Task GiveAccessAsync(GiveAccessGraphDto dto);
    Task ChangeUserAccess(ChangeUserAccessDto dto);
    Task RevokeAccess(RevokeGraphAccessDto dto);
    Task<List<GetAllUserGraphAccessDto>?> AllWithAccessToOperationGraphAsync(int graphId);
    Task<List<UserGetWithSubdivisionDto>?> AllWithoutAccessToOperationGraphAsync(int graphId);
    Dictionary<int, string> AccessTypes();
}