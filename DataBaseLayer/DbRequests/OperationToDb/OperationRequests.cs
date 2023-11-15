using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Options.OperationO;
using Shared.Dto.Operation;
using Shared.Dto.Operation.Filters;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace DatabaseLayer.DbRequests.OperationToDb;

public class OperationRequests
{
    private readonly DbSet<Operation> _operation;

    public OperationRequests(DbContext context)
    {
        _operation = context.Set<Operation>();
    }

    public async Task<List<OperationDto>?> GetAllAsync(GetAllOperationFilters filters, IMapper dataMapper)
    {
        try
        {
            return await _operation
                .SearchOptions(filters.Text, filters.SearchOptions)
                .OrderOptions(filters.OrderOptions, filters.KindOfOrder)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<OperationDto>(dataMapper.ConfigurationProvider)
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