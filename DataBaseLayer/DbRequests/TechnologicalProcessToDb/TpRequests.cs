using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.TechologicalProcessO;
using DB.Model.DetailInfo;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail;
using Shared.Dto.TechnologicalProcess;
using Shared.Static;

namespace DatabaseLayer.DbRequests.TechnologicalProcessToDb;

public class TpRequests
{
    private readonly DbSet<TechnologicalProcess> _techProcess;
    private readonly DbSet<Detail> _detail;
    private readonly IMapper _dataMapper;

    public TpRequests(DbContext context, IMapper dataMapper)
    {
        _techProcess = context.Set<TechnologicalProcess>();
        _detail = context.Set<Detail>();
        _dataMapper = dataMapper;
    }

    public async Task<GetDetailTechProcessesDto?> GetAllDetailTps(int detailId)
    {
        try
        {
            return new()
            {
                Detail = await _detail
                    .Where(d => d.Id == detailId)
                    .ProjectTo<BaseIdSerialTitleDto>(_dataMapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(),

                TechProcesses = await _techProcess
                    .Include(tp => tp.TechnologicalProcessStatuses)
                    .Where(tp => tp.DetailId == detailId)
                    .OrderBy(tp => tp.ManufacturingPriority)
                    .ProjectTo<DetailedTechProcessInfoDto>(_dataMapper.ConfigurationProvider)
                    .ToListAsync()
            };
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

    public async Task<List<GetAllReadonlyTechProcessDto>?> GetAllAsync(GetAllReadonlyTechProcessRequestFilters filters, List<int> detailsId)
    {
        try
        {
            return await _techProcess
                .AsNoTracking()
                .TpReadonlySearch(filters.Text, filters.SearchOptions)
                .TpReadonlyOrder(filters.KindOfOrder, filters.OrderOptions)
                .TpReadonlyFromProduct(detailsId)
                .TpFromBlankType(filters.BlankTypeId)
                .TpFromDetailType(filters.DetailTypeId)
                .TpFromMaterial(filters.MaterialTypeId)
                .TpFromStatus(filters.StatusId)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .ProjectTo<GetAllReadonlyTechProcessDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<GetAllIssuedTechProcessesDto>?> GetAllIssuedTechProcessesAsync(GetAllIssuedTechProcessesRequestFilters filters, List<int> detailsId)
    {
        try
        {
            return await _techProcess
                .AsNoTracking()
                .TpArchiveSearch(filters.Text, filters.SearchOptions)
                .TpArchiveOrder(filters.OrderOptions, filters.KindOfOrder)
                .TpReadonlyFromProduct(detailsId)
                .TpFromBlankType(filters.BlankTypeId)
                .TpFromMaterial(filters.MaterialId)
                .Where(tp => tp.TechnologicalProcessStatuses!.FirstOrDefault(tps => tps.StatusId == (int)TechProcessStatuses.Issued) != null)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .ProjectTo<GetAllIssuedTechProcessesDto>(_dataMapper.ConfigurationProvider)
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