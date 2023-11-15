using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.CUD;
using Shared.Dto.TechnologicalProcess.Status;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Read;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces;

public interface ITpSupervisorService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(AddTechProcessDto tpDto);
    Task ChangeTpStatusAsync(SupervisorChangeTechProcessStatusDto developerStatusDto, int userId);
    Task ChangeProcessDeveloperAsync(ChangeTechProcessDeveloperDto processDeveloperDto);
    Task ChangeTechProcessPriorityAsync(ChangeTechProcessPriorityDto dto);
    Task ReturnToWorkAsync(ReturnInWorkDto dto, int userId);
    Task DeleteAsync(int techProcessId);
    Task<bool> CheckAvailabilityAsync(int detailId);
    Task<Dictionary<int, bool>> CheckRangeAvailabilitiesAsync(List<int> detailsId);
    Task<GetDetailTechProcessesDto?> GetDetailTpsAsync(int detailId);
    Task<List<GetAllDevelopersTasksDto>?> GetAllDevelopersTasksAsync(List<int> developers, int productId);
}
