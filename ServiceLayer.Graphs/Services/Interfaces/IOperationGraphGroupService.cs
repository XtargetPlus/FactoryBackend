using DatabaseLayer.Helper;
using Shared.Dto.Graph.Access;
using Shared.Dto.Graph.CUD;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphGroupService : IErrorsMapper, IDisposable
{
    Task CreateGroupAsync(List<int> operationGraphsId);
    Task AddGraphsAsync(AddGraphsInGroupDto dto);
    Task ChangeGraphInfoAsync(ChangeGraphDto dto);
    Task FreezeOrUnAsync(int mainGraphId, bool freeze);
    Task RegenerationAsync(int mainGraphId);
    Task SwapAsync(SwapGraphDto dto);
    Task ConfirmAsync(int mainGraphId);
    Task UnconfirmAsync(int mainGraphId);
    Task<int> CopyAsync(CopyGraphDto dto);
    Task DeleteAsync(int mainGraphId);
    Task DeleteGraphsAsync(DeleteGraphsInGroupDto dto);
    Task<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>?> GroupAsync(int priority, int userId);
    Task<OpenOperationGraphGroupDto?> OpenAsync(OpenOperationGraphFilters filters, int userId, IOperationGraphService graphService);
}