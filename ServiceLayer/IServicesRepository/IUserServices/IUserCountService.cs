using DatabaseLayer.Options;
using Shared.Enums;

namespace ServiceLayer.IServicesRepository.IUserServices;

public interface IUserCountService : IBaseCountService
{
    Task<int?> GetAllAsync(int statusId);
    Task<int?> GetAllAsync(string text, int statusId, UserSearchOptions searchOptions);
    Task<int?> GetFromSubdivAsync(int subdivisionId);
    Task<int?> GetFromProfessionAsync(int professionId);
}
