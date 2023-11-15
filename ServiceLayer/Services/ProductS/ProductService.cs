using DB;
using DB.Model.ProductInfo;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IProductServices;
using DatabaseLayer.DbRequests.ProductToDb;
using Shared.Dto.Product;
using Shared.Dto.Detail;
using Shared.Dto.Product.Filters;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.ProductS;

/// <summary>
/// Сервис изделий
/// </summary>
public class ProductService : ErrorsMapper, IProductService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Product> _repository;
    private readonly IMapper _dataMapper;

    public ProductService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(BaseProductDto dto)
    {
        var product = await _context.AddWithValidationsAndSaveAsync(new Product { Price = dto.Price, DetailId = dto.DetailId }, this);
        return product?.Id;
    }  

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(ProductDto dto)
    {
        var product = await _repository.FindByIdAsync(dto.ProductId);
        if (product is null)
        {
            AddErrors("Не удалось получить изделие");
            return;
        }
        if (product.Price == dto.Price)
            return;

        product.Price = dto.Price;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Product { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<ProductGetDto?> GetFirstAsync(int id)
    {
        var product = await _repository.FindFirstAsync<ProductGetDto>(filter: p => p.Id == id);
        if (product is null)
            AddErrors("Не удалось получить изделие");
        return product;
    }

    /// <summary>
    /// Получаем список изделий с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список изделий</returns>
    public async Task<List<ProductGetDto>?> GetAllAsync(GetAllProductFilters filters) => await new ProductRequests(_context, _dataMapper).GetAllAsync(filters);

    /// <summary>
    /// Получаем весь список уникальных изделий без фильтров
    /// </summary>
    /// <returns>Список уникальных изделий</returns>
    public async Task<IEnumerable<BaseIdSerialTitleDto>?> GetAllForChoiceAsync() => (await _repository.GetAllAsync<BaseIdSerialTitleDto>(distinct: true))?.Distinct();

    public void Dispose() => _context.Dispose();
}
