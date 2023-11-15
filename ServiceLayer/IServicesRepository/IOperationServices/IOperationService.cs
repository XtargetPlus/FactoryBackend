using DatabaseLayer.Helper;
using Shared.Dto.Operation;
using Shared.Dto.Operation.Filters;

namespace ServiceLayer.IServicesRepository.IOperationServices;

public interface IOperationService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(BaseOperationDto operationDto);
    Task ChangeAsync(OperationDto operationDto);
    Task DeleteAsync(int id);
    Task<List<OperationDto>?> GetAllAsync(GetAllOperationFilters filters);
    Task<OperationDto?> GetFirstAsync(int id);
    Task<List<OperationDto>?> GetAllForChoiceAsync();
}
