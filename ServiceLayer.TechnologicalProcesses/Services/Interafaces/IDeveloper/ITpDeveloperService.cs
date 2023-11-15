using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;

public interface ITpDeveloperService : IErrorsMapper, IDisposable
{
    Task ChangeTpDataInfoAsync(ChangeTechProcessData changeInfoDto);
    Task ChangeTpStatusAsync(BaseChangeTechProcessStatusDto statusDto, int statusId, int userId);
    Task ChangeActualTProcessAsync(ChangeTechProcessActualDto visibilityDto);
}
