using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.EquipmentO;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail;
using Shared.Dto.Equipment.Filters;

namespace DatabaseLayer.DbRequests.EquipmentToDb;

public class EquipmentDetailRequests
{
    private readonly DbSet<EquipmentDetail> _equipmentDetail;
    private readonly IMapper _dataMapper;

    public EquipmentDetailRequests(DbContext context, IMapper dataMapper)
    {
        _equipmentDetail = context.Set<EquipmentDetail>();
        _dataMapper = dataMapper;
    }

    public async Task<List<BaseIdSerialTitleDto>?> GetAllAsync(GetAllEquipmentDetailFilters filters)
    {
        try
        {
            return await _equipmentDetail
                .EquipmentDetailSearch(filters.SearchOptions, filters.Text)
                .EquipmentDetailOrder(filters.OrderOptions, filters.KindOfOrder)
                .Skip(filters.Skip)
                .Take(filters.Take)
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

    public async Task<List<BaseIdSerialTitleDto>?> GetAllFromEquipmentAsync(GetAllEquipmentDetailsFromEquipmentFilters filters)
    {
        try
        {
            return await _equipmentDetail
                .GetEquipmentDetailsByEquipmentId(filters.EquipmentId)
                .EquipmentDetailOrder(filters.OrderOptions, filters.KindOfOrder)
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
}