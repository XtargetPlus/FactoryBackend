using Shared.Dto.TechnologicalProcess;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces;

public interface ITpCountService : IDisposable
{
    Task<int?> GetAllAsync(GetAllReadonlyTechProcessRequestFilters options);
    Task<int?> GetAllIssuedAsync(GetAllIssuedTechProcessesRequestFilters options);
    Task<int?> GetAllIssuedFromTechnologistAsync(int developerId);
    Task<int?> GetAllCompletedDeveloperTpsAsync(int developerId);
}
