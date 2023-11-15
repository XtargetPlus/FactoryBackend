using DatabaseLayer.Options.DetailO;
using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail.Filters;

namespace DatabaseLayer.DbRequests.DetailToDb;

public class DetailCountRequest
{
    private readonly DbSet<Detail> _detail;

    public DetailCountRequest(DbContext context)
    {
        _detail = context.Set<Detail>();
    }

    public async Task<int?> GetAllAsync(GetAllDetailFilters filters, List<int> detailsId)
    {
        try
        {
            return await _detail
                .DetailSearch(filters.SearchOptions, filters.Text)
                .DetailFromDetailType(filters.DetailTypeId)
                .DetailFromProduct(detailsId)
                .DetailFromCompound(filters.CompoundDetailOptions)
                .CountAsync();
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