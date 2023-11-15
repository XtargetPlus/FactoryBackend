using Shared.Dto.Detail.Filters;

namespace ServiceLayer.Details.Services.Interfaces;

public interface IDetailCountService 
{
    Task<int?> GetAllAsync();
    Task<int?> GetAllAsync(int detailTypeId);
    Task<int?> GetAllAsync(GetAllDetailFilters filters);
}
