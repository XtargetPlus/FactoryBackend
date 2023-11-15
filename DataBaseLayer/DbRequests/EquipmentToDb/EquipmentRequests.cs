using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;

namespace DatabaseLayer.IDbRequests.EquipmentToDb;

public class EquipmentRequests
{
    private readonly DbSet<Equipment> _equipment;
    private readonly IMapper _dataMapper;

    public EquipmentRequests(DbContext context, IMapper dataMapper)
    {
        _equipment = context.Set<Equipment>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GetEquipmentDto>?> GetAllAsync(GetAllEquipmentFilters filters)
    {
        try
        {
            return await _equipment
                .EquipmentSearch(filters.SearchOptions, filters.Text)
                .EquipmentOrder(filters.OrderOptions, filters.KindOfOrder)
                .SubdivisionOption(filters.SubdivisionId)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<GetEquipmentDto>(_dataMapper.ConfigurationProvider)
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