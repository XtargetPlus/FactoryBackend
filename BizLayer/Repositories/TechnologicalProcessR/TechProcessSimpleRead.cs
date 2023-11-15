using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StatusInfo;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BizLayer.Repositories.TechnologicalProcessR;

public static class TechProcessSimpleRead
{
    public static async Task<TechnologicalProcess?> GetAsync(BaseModelRequests<TechnologicalProcess> repository, int techProcessId, ErrorsMapper mapper)
    {
        var techProcess = await repository.FindByIdAsync(techProcessId);
        if (techProcess is null)
            mapper.AddErrors("Не удалось получить тех процесс");
        return techProcess;
    }

    public static async Task<TechnologicalProcess?> GetAsync(BaseModelRequests<TechnologicalProcess> repository, int techProcessId, QueryFilterOptions options, ErrorsMapper mapper)
    {
        var techProcess = await repository.FindFirstAsync(
            filter: tpi => tpi.Id == techProcessId,
            addQueryFilters: Convert.ToBoolean((int)options));
        if (techProcess is null)
            mapper.AddErrors("Не удалось получить тех процесс");
        return techProcess;
    }

    public static async Task<int> GetIssuedStatusIdAsync(DbContext context, int techProcessId)
    {
        var techStatusId = await context.Set<TechnologicalProcessStatus>()
            .Where(tps =>
                tps.TechnologicalProcessId == techProcessId &&
                tps.StatusId == (int)TechProcessStatusesForArchive.Issued)
            .Select(tps => tps.Id)
            .SingleOrDefaultAsync();

        return techStatusId;
    }

    public static async Task<Dictionary<int, int>> GetCompletedMainByDetailIdAsync(DbContext context, List<int> detailsId, ErrorsMapper mapper)
    {
        if (detailsId.Count == 0)
            mapper.AddErrors("Не удалось получить список деталей для получения их основных тех процессов");

        var techProcessRepository = context.Set<TechnologicalProcess>();

        var result = await techProcessRepository
            .Where(tp => detailsId.Contains(tp.DetailId) && tp.DevelopmentPriority == 0 && tp.ManufacturingPriority == 1) 
            .Select(tp => new { TechProcessId = tp.Id, tp.DetailId })
            .ToDictionaryAsync(k => k.TechProcessId, v => v.DetailId);

        return result;
    }

    public static async Task<TechnologicalProcess?> GetWithIncludeDataAsync(BaseModelRequests<TechnologicalProcess> repository, int techProcessId, ErrorsMapper mapper)
    {
        var techProcess = await repository.FindFirstAsync(
            filter: tp => tp.Id == techProcessId,
            include: i => i.Include(tp => tp.TechnologicalProcessData));
        if (techProcess is null)
            mapper.AddErrors("Не удалось получить тех процесс с подробной информацией");
        return techProcess;
    }

    public static async Task IncludeStatusesAsync(BaseModelRequests<TechnologicalProcess> repository, TechnologicalProcess techProcess) =>
        await repository.IncludeCollectionAsync(techProcess, tp => tp.TechnologicalProcessStatuses);

    public static async Task<bool> IsIssuedAsync(int techProcessId, DbContext context) =>
        await context.Set<TechnologicalProcessStatus>()
            .Where(tps => tps.TechnologicalProcessId == techProcessId
                          && tps.StatusId == (int)TechProcessStatusesForArchive.Issued)
            .FirstOrDefaultAsync() != null;

    public static async Task<bool> IsSubdivisionHaveTechProcess(int techProcessId, int subdivsionId, DbContext context) =>
        await context.Set<TechnologicalProcessStatus>()
            .Where(tps => tps.TechnologicalProcessId == techProcessId
                          && tps.SubdivisionId == subdivsionId)
            .FirstOrDefaultAsync() != null;
}