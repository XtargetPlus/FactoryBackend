using DatabaseLayer.IDbRequests;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB;
using Shared.Enums;
using ServiceLayer.Equipments.Services.Interfaces;

namespace ServiceLayer.Equipments.Services;

/// <summary>
/// Сервис подсчета количества деталей станков
/// </summary>
public class EquipmentDetailCountService : IEquipmentDetailCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<EquipmentDetail> _count;

    public EquipmentDetailCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество деталей станков без фильтров
    /// </summary>
    /// <returns>Количество</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество деталей станков с учетом текста поиска
    /// </summary>
    /// <param name="text">Текст поиска</param>
    /// <param name="searchOptions">По какому полю происходит поиск</param>
    /// <returns></returns>
    public async Task<int?> GetAllAsync(string text = "", SerialNumberOrTitleFilter searchOptions = SerialNumberOrTitleFilter.ForSerialNumber)
    {
        if (string.IsNullOrEmpty(text))
            return await GetAllAsync();
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => await _count.CountAsync(e => e.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => await _count.CountAsync(e => e.Title.Contains(text)),
            _ => await GetAllAsync()
        };
    }

    public void Dispose() => _context.Dispose();
}
