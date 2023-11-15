using DB.Model.UserInfo;
using DB;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IProfessionServices;

namespace ServiceLayer.Services.ProfessionS;

/// <summary>
/// Сервис подсчета количества профессий с фильтрами и без
/// </summary>
public class ProfessionCountService : IProfessionCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Profession> _count;

    public ProfessionCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество профессий
    /// </summary>
    /// <returns>Количество профессий</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество профессий с учетом текста поиска
    /// </summary>
    /// <param name="text">Текст поиска</param>
    /// <returns>Количество профессий</returns>
    public async Task<int?> GetAllAsync(string text = "") => !string.IsNullOrEmpty(text) ? await _count.CountAsync(p => p.Title.Contains(text)) : await GetAllAsync();

    public void Dispose() => _context.Dispose(); 
}
