using DatabaseLayer.Helper;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.CUD;

namespace ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;

public interface IEquipmentOperationDeveloperService : IErrorsMapper, IDisposable
{
    Task AddAsync(EquipmentOperationDto dto);
    Task ChangeAsync(ChangeEquipmentOperationDto dto);
    Task SwapAsync(SwapEquipmentOperationDto dto);
    Task DeleteAsync(int equipmentOperationId);
    Task InsertBetweenAsync(InsertBetweenEquipmentOperation dto);
}
