using DatabaseLayer.Helper;
using Shared.Dto.Detail;
using Shared.Dto.Detail.Filters;

namespace ServiceLayer.Details.Services.Interfaces;

public interface IDetailService : IErrorsMapper, IDisposable
{
    Task<int?> AddAsync(DetailMoreInfoDto detail);
    Task ChangeAsync(DetailChangeDto detail);
    Task<Dictionary<int, bool>?> IsCompositionsAsync(List<int> detailsId);
    Task DeleteAsync(int detailId);
    Task<GetDetailInfoWithIdDto?> GetFirstAsync(int detailId);
    Task<List<GetDetailDto>?> GetAllAsync(GetAllDetailFilters filters);
    Task<List<DetailProductsDto>?> GetAllProductsAsync(int detailId);
    Task<List<BaseIdSerialTitleDto>?> GetAllProductDetailsAsync(GetAllProductDetailsDto dto);
    Task<DetailListDto?> GetInfoAsync(int detailId);
    Task<string?> GetDetailUnitAsync(int detailId);
}
