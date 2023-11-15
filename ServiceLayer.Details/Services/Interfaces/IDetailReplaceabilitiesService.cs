using DatabaseLayer.Helper;
using Shared.Dto.Detail;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Detail.Filters;

namespace ServiceLayer.Details.Services.Interfaces;

public interface IDetailReplaceabilitiesService : IErrorsMapper, IDisposable
{
    Task<List<BaseIdSerialTitleDto>?> AddAsync(TwoDetailIdDto detailDto);
    Task DeleteAsync(int detailId);
    Task<List<BaseIdSerialTitleDto>?> GetAllAsync(GetAllReplaceabilityFilters filters);
    Task<BaseIdSerialTitleDto?> GetFirstAsync(int detailId);
}
