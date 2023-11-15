using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.ProductInfo;
using ServiceLayer.IServicesRepository.IProductServices;
using Shared.Enums;

namespace ServiceLayer.Services.ProductS;

/// <summary>
/// Сервис подсчета количества изделий с фильтрами и без
/// </summary>
public class ProductCountService : IProductCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Product> _count;

    public ProductCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество изделий 
    /// </summary>
    /// <returns>Количество изделий</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество изделий с фильтрами поиска
    /// </summary>
    /// <param name="text">Текст поиска</param>
    /// <param name="searchOptions">По какому полю сортировать</param>
    /// <returns>Количество изделий</returns>
    public async Task<int?> GetAllAsync(string text = "", SerialNumberOrTitleFilter searchOptions = default)
    {
        if (string.IsNullOrEmpty(text))
            return await GetAllAsync();
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => await _count.CountAsync(p => p.Detail.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => await _count.CountAsync(p => p.Detail.Title.Contains(text)),
            _ => await GetAllAsync()
        };
    }

    public void Dispose() => _count.Dispose();
}
