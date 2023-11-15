using DatabaseLayer.Helper;
using Shared.Dto.Graph.Item;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.BranchesItems;
using Shared.Dto.Graph.Read.Open;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphDetailItemService : IErrorsMapper, IDisposable
{
    Task AddAsync(GraphDetailItemDto dto);
    Task AddToBlockAsync(AddToBlockGraphDetailItemDto dto);
    Task SubstitutionToBranchAsync(SubstitutionToBranchDto dto);
    Task SwapBlocksAsync(SwapGraphDetailItemBlocksDto dto);
    Task SwapInBlockAsync(SwapGraphDetailItemsInBlockDto dto);
    Task InsertBetweenBlocksAsync(GraphDetailItemInsertBetweenBlocksDto dto);
    Task InsertBetweenItemsInBlockAsync(InsertBetweenGraphDetailItemInBlockDto dto);
    Task AddFactCountAsync(AddFactCountDto dto);
    Task DeleteAsync(int detailItemId);
    Task<InterimGraphDetailItemDto?> ByIdAsync(int graphDetailItemId);
    Task<GraphDetailItemHigherDto?> AllAsync(GetAllDetailItemsDto dto);
    Task<List<GetAllToAddToEndDto>?> AllToAddToEndOfMainAsync(AddToEndOfMainInfoDto dto);
    Task<List<GetAllToAddToEndDto>?> AllToAddToEndOfBranchAsync(GetAllToAddToEndOfBranchDto dto);
    Task<List<GetAllBranchesItemsDto>?> AllBranchesItemsAsync(BranchesItemsInfoDto dto);
}