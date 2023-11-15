using DatabaseLayer.Options.TechologicalProcessO;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.TechnologicalProcess;
using Shared.Static;

namespace DatabaseLayer.DbRequests.TechnologicalProcessToDb;

public class TpCountRequests
{
    private readonly DbSet<TechnologicalProcess> _techProcess;

    public TpCountRequests(DbContext context)
    {
        _techProcess = context.Set<TechnologicalProcess>();
    }

    public async Task<int?> GetAllAsync(GetAllReadonlyTechProcessRequestFilters options, List<int> detailsId)
    {
        try
        {
            return await _techProcess.TpReadonlySearch(options.Text, options.SearchOptions)
                .TpReadonlyFromProduct(detailsId)
                .TpFromBlankType(options.BlankTypeId)
                .TpFromDetailType(options.DetailTypeId)
                .TpFromMaterial(options.MaterialTypeId)
                .TpFromStatus(options.StatusId)
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

    public async Task<int?> GetAllIssuedAsync(GetAllIssuedTechProcessesRequestFilters options, List<int> detailsId)
    {
        try
        {
            return await _techProcess.TpArchiveSearch(options.Text, options.SearchOptions)
                .TpReadonlyFromProduct(detailsId)
                .TpFromBlankType(options.BlankTypeId)
                .TpFromMaterial(options.MaterialId)
                .Where(tp => TechProcessVariables.IssuedStatusesId.Contains(tp.TechnologicalProcessStatuses!.OrderByDescending(tps => tps.StatusDate).First().StatusId))
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