using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.Read;
using Shared.Dto.TechnologicalProcess.Filters;
using Shared.Dto.TechnologicalProcess.Issue.Filters;
using Shared.Dto.TechnologicalProcess.Issue.Read;
using Shared.Dto.TechnologicalProcess.Read;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Filters;
using Shared.Enums;
using Shared.Static;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces;

public interface ITpReadonlyService : IErrorsMapper, IDisposable
{
    Task<List<GetAllReadonlyTechProcessDto>?> GetAllTechProcessesAsync(GetAllReadonlyTechProcessRequestFilters filters);
    Task<GetAllEquipmentOperationDto> GetAllEquipmentOperationsAsync(int techProcessItemId);
    Task<List<GeTechProcessDevelopmentStagesDto>?> GetDevelopmentStagesAsync(GetDevelopmentStagesFilters filters);
    Task<GetReadonlyTechProcessInfoDto?> GetInfoAsync(int techProcessId);
    Task<List<GetTechProcessItemDto>?> GetAllTpItemsAsync(GetAllTechProcessItemsFilters filters);
    Task<List<int>?> GetNumberOfBranchesAsync(int techProcessItemId, QueryFilterOptions visibilityOptions = QueryFilterOptions.TurnOn);
    Task<List<GetBaseTechProcessDto>?> GetAllCompletedDeveloperTpsAsync(GetAllCompletedDeveloperTechProcessesFilters filters);
    Task<List<GetExtendedTechProcessDataDto>?> GetAllTpsReadyForDeliveryAsync();
    Task<List<GetAllIssuedTechProcessesDto>?> GetAllIssuedTechProcessesAsync(GetAllIssuedTechProcessesRequestFilters filters);
    Task<List<GetTechProcessDuplicateDto>?> GetAllIssuedDuplicatesTechProcessAsync(GetAllIssuedDuplicatesTechProcessFilters filters);
    Task<List<IssuedTechProcessesFromTechnologistDto>?> GetAllIssuedTechProcessesFromTechnologistAsync(IssuedTechProcessesFromTechnologistRequestFilters filters, int userId);
    Dictionary<int, int[]> GetStatusChangeOptions(List<TechProcessStatuses> statuses);
    Dictionary<int, string> GetStatuses(TechProcessStatusType statusType);
    Task<List<GetDeveloperTasksDto>?> GetDeveloperTasksAsync(int developerId);
}
