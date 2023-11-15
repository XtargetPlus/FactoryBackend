using Shared.Enums;

namespace ServiceLayer.IServicesRepository.IOperationServices;

public interface IOperationCountService : IBaseCountService
{
    Task<int?> GetAllAsync(string text = "", OperationFilterOptions searchOptions = OperationFilterOptions.Base);
}
