using DB.Model.ProductInfo;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Options.ProductO;
using Shared.Dto.Product;
using Shared.Dto.Product.Filters;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace DatabaseLayer.DbRequests.ProductToDb;

public class ProductRequests
{
    private readonly DbSet<Product> _product;
    private readonly IMapper _dataMapper;

    public ProductRequests(DbContext context, IMapper dataMapper)
    {
        _product = context.Set<Product>();
        _dataMapper = dataMapper;
    }

    public async Task<List<ProductGetDto>?> GetAllAsync(GetAllProductFilters filters)
    {
        try
        {
            return await _product
                .ProductOrder(filters.OrderOptions, filters.KindOfOrder)
                .ProductSearch(filters.Text, filters.ProductSearch)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<ProductGetDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ничего не найдено: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
        }
        return null;
    }
}