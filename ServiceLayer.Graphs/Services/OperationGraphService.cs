using AutoMapper;
using BizLayer.Builders.GraphBuilders;
using BizLayer.Repositories.GraphR;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using BizLayer.Repositories.GraphR.GraphDetailR;
using BizLayer.Repositories.GraphR.GraphStatusR;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.DbRequests.GraphToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.StorageInfo.Graph;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.CUD;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;
using Shared.Enums;
using Shared.Static;

namespace ServiceLayer.Graphs.Services;

/// <summary>
/// Сервис операционных графиков графиков 
/// </summary>
public class OperationGraphService : ErrorsMapper, IOperationGraphService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraph> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление нового графика в самый конец списка (Список актуальных графиков: в разработке, активные или приостановленные)
    /// </summary>
    /// <param name="dto">Данные графика для его создания</param>
    /// <param name="ownerId">Создатель(владелец) графика</param>
    /// <returns>Id созданного графика или null (ошибки с предупреждениями)</returns>
    public async Task<int?> AddAsync(AddGraphDto dto, int ownerId)
    {
        var graphBuilder = new OperationGraphBuilder(dto);
        graphBuilder.SetOwner(ownerId);

        var priority = _repository
            .GetAll(og => og.OwnerId == ownerId && OperationGraphVariables.StatusesWithPriority.Contains(og.StatusId))
            .ToList()
            .MaxBy(og => og.Priority)
            ?.Priority + 1 ?? 1;

        graphBuilder.SetPriority(priority);

        if (dto.DetailId != null)
        {
            await graphBuilder.CalculateDetailsInfoAsync(dto.DetailId.Value, dto.PlanCount, _context, _dataMapper, this);
            await graphBuilder.CalculateItemsInfoAsync(_context, _dataMapper, this);
        }

        var operationGraph = graphBuilder.Build();

        if (HasErrors) return null;

        operationGraph = await _context.AddWithValidationsAndSaveAsync(operationGraph, this);
        
        if (HasErrors || HasWarnings) return null;

        return operationGraph!.Id;
    }

    /// <summary>
    /// Редактирование операционного графика
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns></returns>
    public async Task ChangeAsync(ChangeGraphDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.OperationGraphId, this);
        if (HasErrors) return;

        graph!.SubdivisionId = dto.SubdivisionId;
        graph.GraphDate = dto.GraphDate;
        graph.Note = dto.Note;

        if (graph.ProductDetailId.HasValue && graph.PlanCount != dto.PlanCount)
        {
            var graphDetailRepository = new OperationGraphDetailRepository();
            var newPlanCount = dto.PlanCount;

            graph = await _repository.IncludeCollectionAsync(graph, g => g.OperationGraphDetails!);

            var newPlannedNumbers = graphDetailRepository.RecalculatePlannedNumber(
                graph!.OperationGraphDetails!.ToDictionary(d => d.Id, d => d.UsabilityWithFathers),
                newPlanCount
            );
            var newTotalPlannedNumbers = graphDetailRepository.RecalculatePlannedNumber(
                graph.OperationGraphDetails!
                    .Where(d => d.TotalPlannedNumber.HasValue)
                    .ToDictionary(d => d.Id, d => d.UsabilitySum!.Value),
                newPlanCount
            );

            graph.OperationGraphDetails!.ForEach(d => d.PlannedNumber = newPlannedNumbers[d.Id]);
            graph.OperationGraphDetails.ForEach(d => d.TotalPlannedNumber = newTotalPlannedNumbers[d.Id]);
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Заморозка (пауза) разработки операционного графика 
    /// </summary>
    /// <param name="graphId">Id графика, который нужно заморозить</param>
    /// <param name="freeze">true - заморозить, false - разморозить</param>
    /// <returns></returns>
    public async Task FreezeOrUnAsync(int graphId, bool freeze)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, graphId, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        graph!.StatusId = freeze ? (int)GraphStatus.Paused : (int)GraphStatus.InWork;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Перегенерация графика. Сброс всех изменений и возврат к состаянию после создания
    /// </summary>
    /// <param name="graphId">Id графика, который нужно перегенерировать</param>
    /// <returns></returns>
    public async Task RegenerationAsync(int graphId)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, graphId, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWorkWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var graphBuilder = new OperationGraphBuilder(graph!);
        
        graphBuilder.ClearDetailsInfo();

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        await graphBuilder.CalculateDetailsInfoAsync(graph!.ProductDetailId!.Value, graph.PlanCount, _context, _dataMapper, this);
        await graphBuilder.CalculateItemsInfoAsync(_context, _dataMapper, this);

        graphBuilder.Build();

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Пересчет приоритетов для всех графиков с приоритетом > 0. Было [1, 2, 4, 17], стало [1, 2, 3, 4]
    /// </summary>
    /// <returns></returns>
    public async Task RecalculateAllGraphsPrioritiesAsync()
    {
        var graphs = await OperationGraphRead.AllWithPriorityAsync(_repository, this);
        if (HasErrors) return;

        for (var i = 0; i < graphs!.Count; i++)
        {
            graphs[i].Priority = i + 1;
            graphs[i].OperationGraphMainGroups!.ForEach(mg => mg.OperationGraphNext!.Priority = i + 1);
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Подтверждение операционного графика, вместе со всеми его деталями
    /// </summary>
    /// <param name="graphId">Id графика для подтверждения</param>
    /// <returns></returns>
    public async Task ConfirmAsync(int graphId)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, graphId, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWork(graph, this);
        if (HasErrors) return;

        foreach (var detail in graph!.OperationGraphDetails!.Where(d => !d.IsConfirmed))
        {
            detail.IsConfirmed = true;

            // TODO: резервирования сгд и потока
        }

        graph.IsConfirmed = true;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Снятие подтверждения с операционного графика
    /// </summary>
    /// <param name="graphId">Id графика для подтверждения</param>
    /// <returns></returns>
    public async Task UnconfirmAsync(int graphId)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, graphId, this);

        OperationGraphValidations.GraphInWork(graph, this);
        if (HasErrors) return;

        graph!.IsConfirmed = false;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление графика
    /// </summary>
    /// <param name="graphId">Id графика для удаления</param>
    /// <returns></returns>
    public async Task DeleteAsync(int graphId)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, graphId, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);
        
        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        _repository.Remove(graph);
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Открытие операционного графика
    /// </summary>
    /// <param name="filters">Фильтры открытия</param>
    /// <param name="userId">Кто открывает график</param>
    /// <returns>Информация о графике, включая его детали и их операции</returns>
    public async Task<OpenOperationGraphDto?> OpenAsync(OpenOperationGraphFilters filters, int userId)
    { 
        var graph = await OperationGraphRead.OpenAsync(filters.GraphId, userId, _context, _dataMapper, this);
        var details = await OperationGraphDetailRead.AllForOpenAsync(filters, _context, _dataMapper, this);
        
        if (HasErrors) return null;

        graph!.PossibleStatuses = OperationGraphStatusRead.PossibleStatuses((GraphStatus)graph.CurrentStatusId);
        graph.GraphDetails = details!;

        var result = new OpenOperationGraphDto
        {
            GraphInfo = graph,
            IsStatusConfirmed = await OperationGraphRead.IsStatusConfirmedAsync(_context, filters.GraphId)
        };

        if (details!.Count == 0) return result;

        var openItemBuilder = new OpenOperationGraphItemBuilder(
            details
                .Where(d => d.TotalPlanCount.HasValue)
                .Select(d => new { Key = d.GraphDetailId, Value = d.TechProcessId })
                .ToDictionary(k => k.Key, v => v.Value),
            _context,
            _dataMapper,
            this);

        // TODO: Добавить подсчет остатка и сгд (если график не подтвержден)
        
        // TODO: Получение остатка и сгд (если график подтвержден) 

        foreach (var dictItem in await openItemBuilder.BuildAsync())
        {
            graph.GraphDetails.Single(d => d.GraphDetailId == dictItem.Key).ItemsInfo = dictItem.Value;
        }

        return result;
    }

    /// <summary>
    /// Получение списка графиков с полным доступом, которые не находятся в группе
    /// </summary>
    /// <param name="userId">Id пользователя, который ходит получить одиночные графики с полным доступом</param>
    /// <returns>Список одиночных графиков</returns>
    public async Task<List<GetAllSinglesOperationGraphDto>?> AllSinglesForGroupAsync(GetAllSinglesForGroupFilters filters, int userId) =>
        await new OperationGraphRequests(_context, _dataMapper).GetAllSinglesForGroupAsync(filters, userId);

    /// <summary>
    /// Получение списка операционных графиков с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <param name="userId">Пользователь, который запрашивает список операционных графиков</param>
    /// <returns>Список операционных графиков</returns>
    public async Task<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>>?> AllAsync(GetAllOperationGraphFilters filters, int userId)
    {
        var graphs = await new OperationGraphRequests(_context, _dataMapper).GetAllAsync(filters, userId);

        var result = graphs
            ?.Where(graph => graph.IsMainGraph)
            .Select(graph => new GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>
            {
                Priority = graph.Priority,
                IsGroup = graphs.Where(g => g.Priority == graph.Priority).ToList().Count > 1,
                MainGraphId = graphs.Single(g => g.Priority == graph.Priority && g.IsMainGraph).OperationGraphId,
                Graphs = graphs.Where(g => g.Priority == graph.Priority).ToList()
            })
            .ToList();

        result = filters is { KindOfOrder: KindOfOrder.Down } 
            ? result!.OrderByDescending(obj => obj.Priority).ToList() 
            : result!.OrderBy(obj => obj.Priority).ToList();

        return result;
    }
   
    /// <summary>
    /// Получение списка собственных операционных графиков с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки, наличие изделия и какие графики нужно иганировать</param>
    /// <param name="userId">id владельца графиков</param>
    /// <returns>Возврат списка собственных операционных графиков, которые находятся в разработке или на паузе</returns>
    public async Task<List<GetAllOperationGraphDictionaryDto<GetAllOperationGraphFromOwnerDto>>?> AllFromOwnerAsync(GetAllOperationGraphFromOwnerFilters filters, int userId)
    {
        var graphs = await new OperationGraphRequests(_context, _dataMapper).GetAllFromOwnerAsync(filters, userId);

        return graphs
            ?.Where(graph => graph.IsMainGraph)
            .Select(graph => new GetAllOperationGraphDictionaryDto<GetAllOperationGraphFromOwnerDto>
            {
                Priority = graph.Priority,
                IsGroup = graphs.Where(g => g.Priority == graph.Priority).ToList().Count > 1,
                MainGraphId = graphs.Single(g => g.Priority == graph.Priority && g.IsMainGraph).OperationGraphId,
                Graphs = graphs.Where(g => g.Priority == graph.Priority).ToList()
            })
            .ToList();
    }

    public void Dispose() => _context.Dispose();
}