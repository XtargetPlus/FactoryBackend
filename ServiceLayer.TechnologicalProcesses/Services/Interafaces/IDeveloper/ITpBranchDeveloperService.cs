using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;

public interface ITpBranchDeveloperService : IErrorsMapper, IDisposable
{
    Task<(int techProcessItemId, int branch)?> AddAsync(AddBranchWithItemDto tpItemBranch);
    Task FromMainToNewBranchAsync(FromMainToNewBranchDto branchDto);
    Task FromMainToBranchAsync(FromMainToBranchDto branchDto);
    Task GetBranchToMainAsync(BaseBranchDto branchDto);
    Task GetBranchItemToMainAsync(GetBranchItemToMainDto dto);
    Task SwapAsync(SwapBranchesDto branchesDto);
    Task InsertBetweenAsync(InsertBetweenBranchDto branchesDto);
    Task HideOrUncoverAsync(HideOrUncoverBranchDto branchDto);
    Task DeleteWithItemsAsync(BaseBranchDto branchDto);
}
