using AutoMapper;
using AutoMapper.QueryableExtensions;
using DB.Model.DetailInfo;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Read.Open;

namespace DatabaseLayer.DbRequests.GraphToDb;

public class OperationGraphDetailItemRequests
{
    private readonly DbSet<OperationGraphDetailItem> _operationGraphDetail;
    private readonly IMapper _dataMapper;

    public OperationGraphDetailItemRequests(DbContext context, IMapper dataMapper)
    {
        _operationGraphDetail = context.Set<OperationGraphDetailItem>();
        _dataMapper = dataMapper;
    }

    public async Task<Dictionary<int, List<ReadGraphDetailItemDto>>?> ItemsForOpenAsync(List<int> detailIds)
    {
        try
        {
            var items = await _operationGraphDetail
                .AsNoTracking()
                .Where(i => detailIds.Contains(i.OperationGraphDetailId))
                .OrderBy(i => i.OrdinalNumber)
                .ProjectTo<InterimGraphDetailItemDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();

            var result = new Dictionary<int, List<ReadGraphDetailItemDto>>();

            foreach (var detailId in items.Select(i => i.GraphDetailId).Distinct())
            {
                result[detailId] = _dataMapper.Map<List<ReadGraphDetailItemDto>>(items.Where(i => i.GraphDetailId == detailId));
            }

            return result;
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