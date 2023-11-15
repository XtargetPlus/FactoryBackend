using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces;

public interface ITpArchiveService : IErrorsMapper, IDisposable
{
    Task IssueAsync(ArchiveChangeTechProcessStatusDto archiveStatusDto, int userId);
    Task IssueDuplicateAsync(IssueTechProcessDuplicateDto issueDuplicateDto);
    Task DeleteIssuedDuplicateAsync(int techProcessStatusId);
}
