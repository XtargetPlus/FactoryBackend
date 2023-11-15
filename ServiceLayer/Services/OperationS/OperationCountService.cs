using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.TechnologicalProcessInfo;
using ServiceLayer.IServicesRepository.IOperationServices;
using Shared.Enums;

namespace ServiceLayer.Services.OperationS;

/// <summary>
/// Сервис подсчета количества операций с фильтрами и без
/// </summary>
public class OperationCountService : IOperationCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Operation> _count;

    public OperationCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество операций
    /// </summary>
    /// <returns>Количество операций</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество операций с учетом фильтров выборки
    /// </summary>
    /// <param name="text">Текст поиска</param>
    /// <param name="searchOption">По какому полю производится поиск</param>
    /// <returns>Количество операций с учетом фильтров</returns>
    public async Task<int?> GetAllAsync(string text = "", OperationFilterOptions searchOption = OperationFilterOptions.Base)
    {
        if (string.IsNullOrEmpty(text)) 
            return await GetAllAsync();
        return searchOption switch
        {
            OperationFilterOptions.ForShortName => await _count.CountAsync(filter: o => o.ShortName.Contains(text)),
            OperationFilterOptions.ForFullName => await _count.CountAsync(filter: o => o.FullName.Contains(text)),
            _ => await GetAllAsync(),
        };
    }

    public void Dispose() => _context.Dispose();
}
