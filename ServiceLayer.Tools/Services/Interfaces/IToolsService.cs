using DatabaseLayer.Helper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolsService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(AddToolDto dto);
    Task ChangeAsync(ChangeToolDto dto);
    Task DeleteAsync(int id);

    Task<List<GetToolDto>?> GetForCatalogAsync(int? catalogId);
    Task<List<GetToolDto>?> GetWithParameters(GetToolWithParametersDto dto);
}