using DatabaseLayer.Helper;
using Shared.Dto.Detail.DetailChild.Filters;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolParameters.Filters;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolParameterConcreteService : IErrorsMapper, IDisposable
{
    Task AddRangeParametersAsync(AddToolParametersListDto dto);
    Task ChangeRangeParametersAsync(AddToolParametersListDto dto);
    Task DeleteRangeParametersAsync(DeleteToolParametersDto dto);
    Task<List<GetToolParametersDto>?> GetAllRangeParametersAsync(GetAllParametersFilters dto);
}