using DB;
using DB.Model.DetailInfo;
using DatabaseLayer.IDbRequests;
using Shared.Dto.Detail.Filters;
using DatabaseLayer.DbRequests.DetailToDb;
using ServiceLayer.Details.Services.Interfaces;
using BizLayer.Repositories.DetailR;
using AutoMapper;
using Shared.Dto.Detail;

namespace ServiceLayer.Details.Services;

/// <summary>
/// Сервис подсчета количества деталей с фильтрами или без
/// </summary>
public class DetailCountService : IDetailCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Detail> _count;
    private readonly IMapper _dataMapper;

    public DetailCountService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _count = new(_context);
        _dataMapper = dataMapper;
    }

    /// <summary>
    /// Получаем количество деталей
    /// </summary>
    /// <returns>Количество деталей</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    /// <summary>
    /// Получаем количество деталей с учетом типа детали
    /// </summary>
    /// <param name="detailTypeId">Id типа детали</param>
    /// <returns>Количество деталей</returns>
    public async Task<int?> GetAllAsync(int detailTypeId) => detailTypeId > 0 ? await _count.CountAsync(d => d.DetailTypeId == detailTypeId) : await GetAllAsync();

    /// <summary>
    /// Получаем количество деталей с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Количество деталей</returns>
    public async Task<int?> GetAllAsync(GetAllDetailFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId > 0)
            productDetailsId = (await new DetailRepository(new(), _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = false
            }, _context) ?? new()).Select(d => d.DetailId).ToList();
        return await new DetailCountRequest(_context).GetAllAsync(filters, productDetailsId);
    }

    public void Dispose() => _context.Dispose();
}
