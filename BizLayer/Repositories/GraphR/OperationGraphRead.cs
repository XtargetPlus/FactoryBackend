using AutoMapper;
using DatabaseLayer.DbRequests.GraphToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR;

public static class OperationGraphRead
{
    /// <summary>
    /// Получение графика по id
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="graphId">Id графика</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>График (возможен null, значит есть ошибка)</returns>
    public static async Task<OperationGraph?> ByIdAsync(DbContext context, int graphId, ErrorsMapper mapper)
    {
        var result = await context.Set<OperationGraph>().FirstOrDefaultAsync(g => g.Id == graphId);
        if (result is null)
            mapper.AddErrors("Не удалось получить операционный график");
        
        return result;
    }

    /// <summary>
    /// Получение информации о графике при открытии
    /// </summary>
    /// <param name="graphId">Id графика</param>
    /// <param name="userId">Id пользователя, который открывает график, нужен для того, чтобы узнать уровень доступа</param>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="dataMapper">Маппер данных</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Информация о графике (возможен null, значит есть ошибка)</returns>
    public static async Task<GraphInfoDto?> OpenAsync(int graphId, int userId, DbContext context, IMapper dataMapper, ErrorsMapper mapper)
    {
        var result = await new OperationGraphRequests(context, dataMapper).OpenAsync(graphId, userId);
        if (result is null)
            mapper.AddErrors("Не удалось получить операционный график");

        return result;
    }

    /// <summary>
    /// Получение списка графиков по приоритету
    /// </summary>
    /// <param name="repository">Репозиторий графика</param>
    /// <param name="priority">Приоритет графика</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Список графиков (возможен null, значит есть ошибка)</returns>
    public static async Task<List<OperationGraph>?> ByPriorityAsync(BaseModelRequests<OperationGraph> repository, int priority, ErrorsMapper mapper)
    {
        var result = await repository.GetAllAsync(filter: g => g.Priority == priority, trackingOptions: TrackingOptions.WithTracking);
        if (result is null)
            mapper.AddErrors("Не удалось получить операционные графики");

        return result;
    }

    /// <summary>
    /// Получаем список main графиков, с их группами графиков. Учитываются только активные графики, priority > 0
    /// </summary>
    /// <param name="repository">Репозиторий графиков</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Список графиков (возможен null, значит есть ошибка)</returns>
    public static async Task<List<OperationGraph>?> AllWithPriorityAsync(BaseModelRequests<OperationGraph> repository, ErrorsMapper mapper)
    {
        var result = await repository.GetAllAsync(
            filter: g => g.Priority > 0 && g.OperationGraphNextGroups!.Count == 0,
            include: i => 
                i.Include(g => g.OperationGraphNextGroups)
                    .Include(g => g.OperationGraphMainGroups!).ThenInclude(mg => mg.OperationGraphNext!),
            trackingOptions: TrackingOptions.WithTracking);
        if (result is null)
            mapper.AddErrors("Не удалось получить список графиков");

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <returns></returns>
    public static async Task<List<int>> ChildGraphIdsAsync(DbContext context, int graphId) =>
        await context.Set<OperationGraphGroup>()
            .Where(gg => gg.OperationGraphMainId == graphId)
            .Select(gg => gg.OperationGraphNextId)
            .ToListAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <returns></returns>
    public static async Task<int> FatherGraphIdAsync(DbContext context, int graphId) =>
        await context.Set<OperationGraphGroup>()
            .Where(gg => gg.OperationGraphNextId == graphId)
            .Select(gg => gg.OperationGraphMainId)
            .FirstOrDefaultAsync();

    /// <summary>
    /// Получаем список графиков вместе с их деталями, информацией о группах (Дочерние и главная) и список пользователей с доступом
    /// </summary>
    /// <param name="repository">Репозиторий графика</param>
    /// <param name="graphIds">Список Id графиков, которые нужно получить</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Список графиков или null вместе с ошибкой</returns>
    public static async Task<List<OperationGraph>?> RangeWithGroupAndUsersByIdesAsync(BaseModelRequests<OperationGraph> repository, List<int> graphIds, ErrorsMapper mapper)
    {
        var graphs = await repository.GetAllAsync(
            filter: og => graphIds.Contains(og.Id),
            include: i => 
                i.Include(og => og.OperationGraphDetails!)
                    .Include(og => og.OperationGraphMainGroups!)
                    .Include(og => og.OperationGraphNextGroups!)
                    .Include(og => og.OperationGraphUsers!),
            trackingOptions: TrackingOptions.WithTracking
        );
        
        if (graphs is not null && graphs.Count == graphIds.Count) 
            return graphs;
        mapper.AddErrors("Не удалось получить операционные графики");
        return null;

    }

    /// <summary>
    /// Загружаем группу main графика
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="graph">Main график</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Необходима проверка на ошибки</returns>
    /// <exception cref="ArgumentNullException">Ошибка при выгрузке</exception>
    public static async Task LoadGroupGraphsAsync(DbContext context, OperationGraph? graph, ErrorsMapper mapper)
    {
        if (graph is null)
            return;

        graph = await new BaseModelRequests<OperationGraph>(context, null).IncludeCollectionAsync(graph, g => g.OperationGraphMainGroups!);
        if (graph is null)
            mapper.AddErrors("Не удалось загрузить графики группы");

        var graphGroupRepository = new BaseModelRequests<OperationGraphGroup>(context, null);

        for (var i = 0; i < graph?.OperationGraphMainGroups!.Count; i++)
        {
            graph.OperationGraphMainGroups![i] = await graphGroupRepository.InculdeReferenceAsync(
                graph.OperationGraphMainGroups[i],
                g => g.OperationGraphNext) 
                                                 ?? throw new ArgumentNullException("Не удалось выгрузить связанные графики ", nameof(graph));
        }
    }

    /// <summary>
    /// Загружаем список пользователей с доступом
    /// </summary>
    /// <param name="repository">Репозиторий графика</param>
    /// <param name="graph">График</param>
    /// <param name="mapper">Маппер ошибок</param>
    /// <returns>Необходима проверка на ошибки</returns>
    /// <exception cref="ArgumentNullException">Ошибка при выгрузке</exception>
    public static async Task LoadUsersWithAccessAsync(BaseModelRequests<OperationGraph> repository, OperationGraph? graph, ErrorsMapper mapper)
    {
        if (graph is null)
            return;

        graph = await repository.IncludeCollectionAsync(graph, g => g.OperationGraphUsers);
        if (graph is null)
            mapper.AddErrors("Не удалось загрузить пользователей с правами");

        for (var i = 0; i < graph?.OperationGraphMainGroups!.Count; i++)
        {
            graph.OperationGraphMainGroups![i].OperationGraphNext = await repository.IncludeCollectionAsync(
                graph.OperationGraphMainGroups[i].OperationGraphNext!,
                g => g.OperationGraphUsers) ?? throw new ArgumentNullException("Не удалось выгрузить пользователей с правами ", nameof(graph));
        }
    }
    
    /// <summary>
    /// Получаем последний приоритет графика с учетом игнорируемых
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="ignoredPriorities">Список игнорируемых приоритетов</param>
    /// <returns>Последний приоритет</returns>
    public static async Task<int> LastOrDefaultPriorityAsync(DbContext context, List<int> ignoredPriorities) =>
        await context.Set<OperationGraph>()
            .Where(g => !ignoredPriorities.Contains(g.Priority))
            .OrderBy(g => g.Priority)
            .Select(g => g.Priority)
            .LastOrDefaultAsync();

    /// <summary>
    /// Проверка на то, подтвержден ли статус графика
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="graphId">Id графика</param>
    /// <returns>True - подтвержден, False - не подтвержден</returns>
    public static async Task<bool> IsStatusConfirmedAsync(DbContext context, int graphId)
    {
        var graph = await context.Set<OperationGraph>().Where(g => g.Id == graphId).SingleAsync();

        return (GraphStatus)graph.StatusId is not (GraphStatus.Active or GraphStatus.Canceled) || graph.ConfigrmingId.HasValue;
    }

    /// <summary>
    /// Получаем план графика
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="graphId">Id графика</param>
    /// <returns>План графика</returns>
    public static async Task<float> PlanCountAsync(DbContext context, int graphId) =>
        await context.Set<OperationGraph>().Where(g => g.Id == graphId).Select(g => g.PlanCount).FirstOrDefaultAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="graphId"></param>
    /// <returns></returns>
    public static async Task<List<int>> GroupIdsAsync(DbContext context, int graphId)
    {
        var graphGroupSet = context.Set<OperationGraphGroup>();

        var result = await graphGroupSet.Where(gg => gg.OperationGraphMainId == graphId).Select(gg => gg.OperationGraphNextId).ToListAsync();
        result.AddRange(await graphGroupSet.Where(gg => gg.OperationGraphNextId == graphId).Select(gg => gg.OperationGraphMainId).ToListAsync());
        result.Add(graphId);

        return result;
    }
}