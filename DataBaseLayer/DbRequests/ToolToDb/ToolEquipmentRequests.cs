using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.ToolO;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolEquipment.Filters;

namespace DatabaseLayer.DbRequests.ToolToDb;

public class ToolEquipmentRequests
{
    private readonly DbSet<EquipmentTool> _equipmentTool;
    private readonly IMapper _dataMapper;

    public ToolEquipmentRequests(DbContext context, IMapper mapper)
    {
        _equipmentTool = context.Set<EquipmentTool>();
        _dataMapper = mapper;
    }

    public async Task<List<GetToolEquipmentDto>?> GetAllEquipmentAsync(GetAllEquipmentFilters filters)
    {
        try
        {
            return await _equipmentTool
                .AsNoTracking()
                .ToolEquipmentOrder(filters.OrderOption, filters.kindOfOrder)
                .Where(eq => eq.ToolId == filters.Id)
                .ProjectTo<GetToolEquipmentDto>(_dataMapper.ConfigurationProvider)
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