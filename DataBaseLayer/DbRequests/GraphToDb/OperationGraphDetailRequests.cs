using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.GraphO;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace DatabaseLayer.DbRequests.GraphToDb;

public class OperationGraphDetailRequests
{
    private readonly DbSet<OperationGraphDetail> _operationGraphDetail;
    private readonly IMapper _dataMapper;

    public OperationGraphDetailRequests(DbContext context, IMapper dataMapper)
    {
        _operationGraphDetail = context.Set<OperationGraphDetail>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GraphDetailDto>?> GetDetailsForOpenAsync(OpenOperationGraphFilters filters)
    {
        try
        {
            return await _operationGraphDetail
                .AsNoTracking()
                .Where(d => d.OperationGraphId == filters.GraphId)
                .GraphOpen(filters.GraphOpenType)
                .Visibility(filters.DetailVisibility)
                .ProjectTo<GraphDetailDto>(_dataMapper.ConfigurationProvider, new { openWithRepeats = filters.GraphOpenType == GraphOpenType.WithRepeats})
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