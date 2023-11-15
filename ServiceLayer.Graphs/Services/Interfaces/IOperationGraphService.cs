using DatabaseLayer.Helper;
using Shared.Dto.Graph.CUD;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.Open;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(AddGraphDto dto, int ownerId);
    Task ChangeAsync(ChangeGraphDto dto);
    Task FreezeOrUnAsync(int graphId, bool freeze);
    Task RegenerationAsync(int graphId);
    Task RecalculateAllGraphsPrioritiesAsync();
    Task ConfirmAsync(int graphId);
    Task UnconfirmAsync(int graphId);
    Task DeleteAsync(int graphId);
    Task<OpenOperationGraphDto?> OpenAsync(OpenOperationGraphFilters filters, int userId);
    Task<List<GetAllSinglesOperationGraphDto>?> AllSinglesForGroupAsync(GetAllSinglesForGroupFilters filters, int userId);
    Task<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>>?> AllAsync(GetAllOperationGraphFilters filters, int userId);
    Task<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphFromOwnerDto>>?> AllFromOwnerAsync(GetAllOperationGraphFromOwnerFilters filters, int userId);
}