using DB.Model.SubdivisionInfo.EquipmentInfo;
using DatabaseLayer.IDbRequests;
using ServiceLayer.Equipments.Services.Interfaces;
using DB;
using Shared.Enums;

namespace ServiceLayer.Equipments.Services;

/// <summary>
/// Сервис подсчета количества станков с фильтрами или без
/// </summary>
public class EquipmentCountService : IEquipmentCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Equipment> _count;

    public EquipmentCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Количество станков без фильтров
    /// </summary>
    /// <returns>Количество станков</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество станков подразделения
    /// </summary>
    /// <param name="subdivisionId">Id подразделения</param>
    /// <returns>Количество</returns>
    public async Task<int?> GetAllAsync(int subdivisionId) =>
        subdivisionId > 0 ? await _count.CountAsync(e => e.SubdivisionId == subdivisionId) : await GetAllAsync();

    /// <summary>
    /// Получаем количество станков с учетом текста поиска и подразделения
    /// </summary>
    /// <param name="subdivisionId">Id подразделения</param>
    /// <param name="text">Текст поиска</param>
    /// <param name="searchOptions">По какому полю производится поиск</param>
    /// <returns>Количество</returns>
    public async Task<int?> GetAllAsync(int subdivisionId = 0, string text = "", SerialNumberOrTitleFilter searchOptions = SerialNumberOrTitleFilter.ForSerialNumber)
    {
        if (subdivisionId <= 0 && text is "" or null)
            return await GetAllAsync();

        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => await _count.CountAsync(e => subdivisionId > 0 && text != string.Empty
                                                                                        ? e.SerialNumber.Contains(text) && e.SubdivisionId == subdivisionId      
                                                                                        : subdivisionId <= 0 
                                                                                            ? e.SerialNumber.Contains(text)
                                                                                            : e.SubdivisionId == subdivisionId),
            SerialNumberOrTitleFilter.ForTitle => await _count.CountAsync(e => subdivisionId > 0 && text != string.Empty
                                                                                        ? e.Title.Contains(text) && e.SubdivisionId == subdivisionId
                                                                                        : subdivisionId <= 0
                                                                                            ? e.Title.Contains(text)
                                                                                            : e.SubdivisionId == subdivisionId),
            _ => await GetAllAsync(subdivisionId)
        };
    }

    public void Dispose() => _context.Dispose();
}
