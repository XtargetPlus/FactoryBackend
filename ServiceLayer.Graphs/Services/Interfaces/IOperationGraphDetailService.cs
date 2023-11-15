using DatabaseLayer.Helper;
using Shared.Dto.Graph.Detail;
using Shared.Dto.Graph.Read.Open;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphDetailService : IErrorsMapper, IDisposable
{
    Task AddAsync(AddGraphDetailDto dto);
    Task ChangeTechProcessAsync(ChangeTechProcessDto dto);
    Task ConfirmAsync(int graphDetailId);
    Task UnconfirmAsync(int graphDetailId);
    Task AddFinishedGoodsInventoryAsync(AddFinishedGoodsInventoryDto dto);
    Task AddCountInStreamAsync(AddCountInStreamDto dto);
    Task HideOrUncoverAsync(int graphDetailId, bool visibility);
    Task ChangeUsabilityAsync(ChangeUsabilityDto dto);
    Task SwapAsync(SwapGraphDetailsDto dto);
    Task InsertBetweenAsync(InsertBetweenGraphDetailsDto dto);
    Task DeleteAsync(int graphDetailId);
    Task<GraphDetailDto?> ByIdAsync(int graphDetailId);
}