using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Options.DetailO;
using Shared.Dto.Detail;
using Shared.Dto.Detail.Filters;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace DatabaseLayer.IDbRequests.DetailToDb;

public class DetailRequests
{
    private readonly DbContext _context;
    private readonly DbSet<Detail> _detail;
    private readonly IMapper _dataMapper;

    public DetailRequests(DbContext context, IMapper dataMapper)
    {
        _context = context;
        _detail = _context.Set<Detail>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GetDetailDto>?> GetAllAsync(GetAllDetailFilters filters, List<int> detailsId)
    {
        try
        {
            return await _detail
                .DetailFromDetailType(filters.DetailTypeId)
                .DetailFromCompound(filters.CompoundDetailOptions)
                .DetailFromProduct(detailsId)
                .DetailSearch(filters.SearchOptions, filters.Text)
                .DetailOrder(filters.OrderOptions, filters.KindOfOrder)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<GetDetailDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<BaseIdSerialTitleDto>?> GetAllReplaceabilitiesAsync(GetAllReplaceabilityFilters filters)
    {
        try
        {
            return await _context.Set<DetailReplaceability>()
                .DetailOrder(filters.OrderOptions, filters.KindOfOrder)
                .Where(d => d.FirstDetailId == filters.DetailId)
                .AsNoTracking()
                .ProjectTo<BaseIdSerialTitleDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<DetailBaseInfoForProductsTreeDto?> FindBaseInfoAsync(int id)
    {
        try
        {
            return await _detail
                .Where(d => d.Id == id)
                .AsNoTracking()
                .ProjectTo<DetailBaseInfoForProductsTreeDto>(_dataMapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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