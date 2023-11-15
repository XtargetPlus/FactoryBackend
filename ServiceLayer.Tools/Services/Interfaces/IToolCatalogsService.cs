using System.ComponentModel;
using DatabaseLayer.Helper;
using Shared.Dto.Tools;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolCatalogsService : IErrorsMapper, IDisposable
{
    Task<int?> AddCatalogAsync(AddToolCatalogDto dto);
    Task ChangeCatalogAsync(ChangeToolCatalogDto dto);
    Task DeleteCatalogAsync(int id);
    Task<List<GetToolCatalogDto>?> GetLevelAsync([DefaultValue(null)] int? fatherId);
    Task ChangeChildCatalogAsync(ChangeToolAndCatalogChild dto);
    Task<OpenCatalogDto> AddKeyAsync(GetAllOpenCatalog dto);
}