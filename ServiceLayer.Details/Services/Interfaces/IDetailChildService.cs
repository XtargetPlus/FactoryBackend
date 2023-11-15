using DatabaseLayer.Helper;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Detail.DetailChild.Filters;

namespace ServiceLayer.Details.Services.Interfaces;

public interface IDetailChildService : IErrorsMapper, IDisposable
{
    Task AddAsync(DetailChildAddDto detailChildrenDto);
    Task ChangeAsync(DetailChildAddDto detailChildrenDto);
    Task InsertBetweenAsync(InsertBetweenChildDto dto);
    Task SwapChildrenNumbersAsync(DetailChildSwapDto detailsDto);
    Task DeleteAsync(TwoDetailIdDto detailChildrenDto);
    Task<GetDetailChildDto?> GetFirstChildAsync(int fatherId, int childId);
    Task<List<GetDetailChildDto>?> GetAllAsync(GetAllChildrenFilters filters);
}
