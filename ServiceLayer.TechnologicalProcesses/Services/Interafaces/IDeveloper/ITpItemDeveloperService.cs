using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.TechProcessItem.CUD;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;

public interface ITpItemDeveloperService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(AddTechProcessItemDto dto);
    Task ChangeAsync(ChangeTechProcessItemDto dto);
    Task SwapAsync(SwapItemsDto dto);
    Task InsertBetweenAsync(InsertBetweenItemsDto dto);
    Task HideWithBranchesItemsAsync(int tpItemId);
    Task<List<GetTechProcessItemDto>?> UncoverWithBranchesItemsAsync(int tpItemId);
    Task DeleteWithBranchesItemsAsync(int tpItemId);
}
