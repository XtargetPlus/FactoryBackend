using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.ToolO;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolReplaceability.Filters;

namespace DatabaseLayer.DbRequests.ToolToDb;

public class ToolReplaceabilityRequests
{
    private readonly DbSet<ToolReplaceability> _toolReplaceability;
    private readonly IMapper _dataMapper;
    private readonly DbContext _context;

    public ToolReplaceabilityRequests(DbContext context, IMapper mapper)
    {
        _context = context;
        _toolReplaceability = context.Set<ToolReplaceability>();
        _dataMapper = mapper;
    }

    public async Task<List<GetToolReplaceabilityDto>?> GetAllReplaceabilityAsync(GetAllReplaceabilityFilters filters)
    {
        try
        {
            return await _context.Set<ToolReplaceability>()
                .ToolOrder(filters.sortOptions, filters.KindOfOrder)
                .Where(t => t.MasterId == filters.ToolId)
                .ProjectTo<GetToolReplaceabilityDto>(_dataMapper.ConfigurationProvider)
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