using DatabaseLayer.Options.DetailO;
using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail.DetailTree;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Detail.DetailChild.Filters;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace DatabaseLayer.IDbRequests.DetailToDb;

public class DetailChildRequests
{
    private readonly DbSet<DetailsChild> _detailChild;
    private readonly IMapper _dataMapper;

    public DetailChildRequests(DbContext context, IMapper dataMapper)
    {
        _detailChild = context.Set<DetailsChild>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GetDetailChildDto>?> GetAllChildrenAsync(GetAllChildrenFilters filters)
    {
        try
        {
            return await _detailChild
                .DetailChildOrder(filters.OrderOptions, filters.KindOfOrder)
                .Where(dc => dc.FatherId == filters.FatherId)
                .AsNoTracking()
                .ProjectTo<GetDetailChildDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<DetailTree>?> GetAllFatherAsync(int childId, int count)
    {
        try
        {
            return await _detailChild
                .Where(dc => dc.ChildId == childId)
                .AsNoTracking()
                .Select(dc => new DetailTree(dc.FatherId)
                {
                    SerialNumber = dc.Father!.SerialNumber,
                    Title = dc.Father.Title,
                    Unit = dc.Father.Unit!.Title,
                    Count = dc.Count * count,
                })
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