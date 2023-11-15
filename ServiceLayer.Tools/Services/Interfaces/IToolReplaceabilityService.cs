using DatabaseLayer.Helper;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolReplaceability.Filters;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolReplaceabilityService : IErrorsMapper, IDisposable
{
    Task AddReplaceabilityAsync(AddReplaceabilityDto dto);
    Task ChangeReplaceabilityAsync(AddReplaceabilityDto dto);
    Task DeleteReplaceabilityAsync(AddReplaceabilityDto dto);

    Task<List<GetToolReplaceabilityDto>?> GetReplaceabilityAsync(GetAllReplaceabilityFilters filters);
}