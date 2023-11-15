using DatabaseLayer.Helper;
using Shared.Dto.Detail;
using Shared.Dto.Product;
using Shared.Dto.Product.Filters;

namespace ServiceLayer.IServicesRepository.IProductServices;

public interface IProductService : IErrorsMapper
{
    Task<int?> AddAsync(BaseProductDto value);
    Task ChangeAsync(ProductDto value);
    Task<List<ProductGetDto>?> GetAllAsync(GetAllProductFilters filters);
    Task DeleteAsync(int id);
    Task<ProductGetDto?> GetFirstAsync(int id);
    Task<IEnumerable<BaseIdSerialTitleDto>?> GetAllForChoiceAsync();
}
