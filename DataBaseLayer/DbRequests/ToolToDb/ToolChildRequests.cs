using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.ToolO;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolChild.Filters;


namespace DatabaseLayer.DbRequests.ToolToDb;

public class ToolChildRequests
{
    private readonly DbSet<ToolChild> _toolChild;
    private readonly IMapper _dataMapper;

    public ToolChildRequests(DbContext context, IMapper mapper)
    {
        _toolChild = context.Set<ToolChild>();
        _dataMapper = mapper;
    }

    public async Task<List<GetToolChildDto>?> GetAllChildrenAsync(GetAllChildrenFilters filters)
    {
        try
        {
            return await _toolChild
                .AsNoTracking()
                .Where(tc => tc.FatherId == filters.FatherId)
                .ToolChildOrder(filters.OrderOptions, filters.KindOfOrder)
                .ProjectTo<GetToolChildDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<GetToolFatherDto>?> GetAllFatherAsync(GetAllFatherFilters filters)
    {
        try
        {
            return await _toolChild
                .AsNoTracking()
                .Where(tc => tc.ChildId == filters.ChildId)
                .ToolChildOrder(filters.OrderOptions, filters.KindOfOrder)
                .ProjectTo<GetToolFatherDto>(_dataMapper.ConfigurationProvider)
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