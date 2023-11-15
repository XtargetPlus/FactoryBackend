using DatabaseLayer.Helper;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolChild.Filters;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolChildService : IErrorsMapper, IDisposable
{
    Task AddChildAsync(AddToolChildrenDto dto);
    Task ChangeChildAsync(ChangeToolChildDto dto);
    Task DeleteChildAsync(DeleteToolChildDto dto);
    Task InsertBetweenAsync(SwapToolChildDto dto);
    Task SwapChildAsync(SwapToolChildDto dto);
    Task<List<GetToolChildDto>?> GetAllAsync(GetAllChildrenFilters dto);
    Task<List<GetToolFatherDto>?> GetAllFatherAsync(GetAllFatherFilters dto);

}