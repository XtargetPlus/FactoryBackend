using DatabaseLayer.Helper;
using Shared.Dto;
using Shared.Dto.Status;

namespace ServiceLayer.IServicesRepository;

public interface IStatusService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(StatusChangeDto value);
    Task ChangeAsync(StatusDto value);
    Task<List<StatusDto>?> GetAllAsync(int take, int skip);
    Task DeleteAsync(int id);
    Task<StatusDto?> GetFirstAsync(int id);
    Task<List<BaseDto>?> GetTableStatusesAsync(string tableName);
}
