using AutoMapper;
using BizLayer.Builders.GraphBuilders;
using BizLayer.Events;
using BizLayer.Events.Graphs;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using DB;
using ServiceLayer.Graphs.Services.Interfaces;
using BizLayer.Repositories.GraphR;
using DatabaseLayer.DatabaseChange;
using Shared.Dto.Graph.CUD;
using Shared.Enums;
using Shared.Dto.Graph.Access;
using BizLayer.Repositories.GraphR.GraphDetailR;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read.Open;
using Shared.Dto.Graph.Read;
using DatabaseLayer.DbRequests.GraphToDb;
using DB.Model.UserInfo;
using Shared.BasicStructuresExtensions;

namespace ServiceLayer.Graphs.Services;

public class OperationGraphGroupService : ErrorsMapper, IOperationGraphGroupService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraph> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphGroupService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Создаем группу графиков
    /// </summary>
    /// <param name="operationGraphsId">Список графиков для группы, первый в списке будет считаться main</param>
    /// <returns></returns>
    public async Task CreateGroupAsync(List<int> operationGraphsId)
    {
        if (operationGraphsId.Count < 2)
        {
            AddErrors("Количество графиков в группе минимум 2");
            return;
        }

        var graphs = await OperationGraphRead.RangeWithGroupAndUsersByIdesAsync(_repository, operationGraphsId, this);
        if (HasErrors)
            return;

        OperationGraphValidations.ValidationGraphsBeforeAddingToGroup(graphs!, this);
        if (HasErrors)
            return;

        // подгружаем операции деталей
        await OperationGraphDetailItemRead.LoadDetailsItemsAsync(graphs!, _context, _dataMapper);

        // выбираем будущий main в графике
        var mainGraph = graphs![0];
        if (!graphs.Remove(graphs[0]))
        {
            AddErrors("Ошибка в вычислениях");
            return;
        }

        var graphGroupBuilder = new OperationGraphGroupBuilder(mainGraph, graphs);
        await graphGroupBuilder.GroupingAsync(_context, this);
        graphGroupBuilder.Build();

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Добавление графиков в существующую группу
    /// </summary>
    /// <param name="dto">Информация для добавление графиков в группу</param>
    /// <returns></returns>
    public async Task AddGraphsAsync(AddGraphsInGroupDto dto)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, dto.MainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        await OperationGraphRead.LoadUsersWithAccessAsync(_repository, mainGraph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, mainGraph, _dataMapper);

        OperationGraphValidations.GraphInWorkWithoutConfirmedDetails(mainGraph, this);
        if (HasErrors) return;

        var newGraphs = await OperationGraphRead.RangeWithGroupAndUsersByIdesAsync(_repository, dto.GraphsId, this) ?? new List<OperationGraph>();
        if (HasErrors) return;

        OperationGraphValidations.ValidationGraphsBeforeAddingToGroup(newGraphs, this);
        if (HasErrors) return;

        await OperationGraphDetailItemRead.LoadDetailsItemsAsync(newGraphs, _context, _dataMapper);

        var graphGroupRepository = new OperationGraphGroupRepository();
        var graphDetailRepository = new OperationGraphDetailRepository();

        // Предоставляем доступ пользователям из main в новые графикк
        newGraphs.ForEach(g => g.OperationGraphUsers!.AddRange(mainGraph!.OperationGraphUsers!.Select(gu => new OperationGraphUser
        {
            OperationGraph = g,
            IsReadonly = gu.IsReadonly,
            UserId = gu.UserId
        })));

        var localGraphs = mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext).ToList();
        localGraphs = localGraphs.Where(g => g.OperationGraphDetails.Count > 0).ToList();
        
        // зависимые детали
        var slaveDetails = await graphDetailRepository.SlaveDetailsInGroupGraphs(_context, mainGraph, localGraphs!);
        var localGraph = mainGraph;

        // получаем последний порядковый номер без повторов
        var numberWithoutRepeats = graphDetailRepository.LastNumberWithoutRepeats(mainGraph);

        // добавляем новые графики в группу
        graphGroupRepository.AddGraphsInGroup(mainGraph, newGraphs);

        newGraphs = newGraphs.Where(g => g.OperationGraphDetails!.Count > 0).ToList();
        while (localGraphs.Count > 0 || newGraphs.Count > 0)
        {
            // получаем повторы в других графиках
            var repeatsDetailsInOtherGraphs = graphDetailRepository.RepeatsDetailsInOtherGraphs(
                localGraph!.OperationGraphDetails!,
                newGraphs
            );

            if (repeatsDetailsInOtherGraphs.Count > 0)
            {
                // идем по деталям из локального main
                graphGroupRepository.AddDetailsInGroup(localGraph, repeatsDetailsInOtherGraphs, slaveDetails);
                // получаем список графиков, в которых есть повторы
                var repeatsGraphs = graphGroupRepository.GraphsWithRepeats(repeatsDetailsInOtherGraphs);
                // проходимся только по графикам с повторами
                graphGroupRepository.ClearRepetitions(newGraphs, repeatsDetailsInOtherGraphs, repeatsGraphs);
            }

            if (localGraphs.Count > 0)
            {
                localGraph = localGraphs[0];
                if (localGraphs.Remove(localGraphs[0])) continue;
            }
            else
            {
                numberWithoutRepeats = graphGroupRepository.AddNewNumberWithoutRepeats(newGraphs[0], numberWithoutRepeats);

                localGraph = newGraphs[0];
                if (newGraphs.Remove(newGraphs[0])) continue;
            }

            AddErrors("Ошибка в вычислениях");
            return;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }
    
    /// <summary>
    /// Редактирование графика в группе
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns></returns>
    public async Task ChangeGraphInfoAsync(ChangeGraphDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.OperationGraphId, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);
        await OperationGraphDetailRead.LoadNextGroupAsync(_context, graph?.OperationGraphDetails, _dataMapper);

        OperationGraphValidations.GraphInWorkWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        var groupEvent = new ChangeGraphInGroupEvent(graph!.PlanCount, dto.PlanCount); 
        groupEvent.SetMainDetails(graph.OperationGraphDetails!);
        groupEvent.SetGroupDetails(graph.OperationGraphDetails!
            .Where(d => d.OperationGraphNextDetails!.Any())
            .Select(d => new
            {
                Key = d.DetailId,
                Value = d.OperationGraphNextDetails!.Single().OperationGraphMainDetail!
            })
            .ToDictionary(k => k.Key, v => v.Value));

        var eventHandler = new LocalEventHandler(groupEvent);
        eventHandler.Execute(_context, _dataMapper, this);

        graph.GraphDate = dto.GraphDate;
        graph.PlanCount = dto.PlanCount;
        graph.Note = dto.Note;
        graph.SubdivisionId = dto.SubdivisionId;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Заморозка или разморозка группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <param name="freeze">true - заморозка, false - разморока</param>
    /// <returns></returns>
    public async Task FreezeOrUnAsync(int mainGraphId, bool freeze)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, mainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        graph!.StatusId = freeze ? (int)GraphStatus.Paused : (int)GraphStatus.InWork;
        graph.OperationGraphMainGroups!.ForEach(mg => mg.OperationGraphNext!.StatusId = freeze ? (int)GraphStatus.Paused : (int)GraphStatus.InWork);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Перегенерация группы графиков (приведение базовому виду, как после создания группы)
    /// </summary>
    /// <param name="mainGraphId">Id главного графика в группе</param>
    /// <returns></returns>
    public async Task RegenerationAsync(int mainGraphId)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, mainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, mainGraph, _dataMapper);

        OperationGraphValidations.GraphInWorkWithoutConfirmedDetails(mainGraph, this);
        if (HasErrors) return;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        // закидываем все графики в группу
        var localGraphs = new List<OperationGraph> { mainGraph! };
        localGraphs.AddRange(mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext)!);

        // пересчитываем информацию группы (детали и операции)
        var graphGroupRepository = new OperationGraphGroupRepository();
        await graphGroupRepository.RecalculateGroupInfoAsync(localGraphs, _context, _dataMapper, this);

        // выносим в список график кроме main
        localGraphs.Remove(mainGraph);
        if (!localGraphs.Any()) return;

        // очищаем связи
        mainGraph.OperationGraphMainGroups!.ForEach(gg => gg.OperationGraphNext!.OperationGraphNextGroups = null);
        mainGraph.OperationGraphMainGroups = null;

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        // создаем группу заново
        var graphGroupBuilder = new OperationGraphGroupBuilder(mainGraph, localGraphs);
        await graphGroupBuilder.GroupingAsync(_context, this);
        graphGroupBuilder.Build();

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Смена местами графиков
    /// </summary>
    /// <param name="dto">Информация для смены местами графиков</param>
    /// <returns></returns>
    public async Task SwapAsync(SwapGraphDto dto)
    {
        var targetGraphs = await OperationGraphRead.ByPriorityAsync(_repository, dto.TargetGroupPriority, this);
        var sourceGraphs = await OperationGraphRead.ByPriorityAsync(_repository, dto.SourceGroupPriority, this);

        if (HasErrors) return;

        targetGraphs!.ForEach(g => g.Priority = dto.SourceGroupPriority);
        sourceGraphs!.ForEach(g => g.Priority = dto.TargetGroupPriority);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Подтверждение группы графиков, вместе с их деталями
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <returns></returns>
    public async Task ConfirmAsync(int mainGraphId)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, mainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, mainGraph, _dataMapper);

        OperationGraphValidations.GraphInWork(mainGraph, this);
        if (HasErrors) return;

        var graphList = new List<OperationGraph> { mainGraph! };
        graphList.AddRange(mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext)!);

        foreach (var graph in graphList.Where(g => !g.IsConfirmed))
        {
            foreach (var detail in graph.OperationGraphDetails!.Where(d => !d.IsConfirmed))
            {
                detail.IsConfirmed = true;

                // TODO: резервирование потока и сгд
            }

            graph.IsConfirmed = true;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Снятие подтверждения с группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика группы</param>
    /// <returns></returns>
    public async Task UnconfirmAsync(int mainGraphId)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, mainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);

        OperationGraphValidations.GraphInWork(mainGraph, this);
        if (HasErrors) return;

        var graphList = new List<OperationGraph> { mainGraph! };
        graphList.AddRange(mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext)!);

        graphList.ForEach(g => g.IsConfirmed = false);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Копирование графика или группы графиков
    /// </summary>
    /// <param name="dto">В GraphId всегда передавать MainGraphId, если это группа графиков</param>
    /// <returns>Id добавленной записи (mainGraphId) или 0</returns>
    public async Task<int> CopyAsync(CopyGraphDto dto)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, dto.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, mainGraph, _dataMapper);
        if (HasErrors) return 0;

        var graphs = new List<OperationGraph> { mainGraph! };
        graphs.AddRange(mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext)!);
        
        foreach (var detail in graphs.SelectMany(graph => graph.OperationGraphDetails!))
            await OperationGraphDetailItemRead.LoadDetailItemsAsync(_context, detail);

        var groupRepostiroy = new OperationGraphGroupRepository();

        var lastPriority = await OperationGraphRead.LastOrDefaultPriorityAsync(_context, new List<int>());
        var newGraphs = groupRepostiroy.Copy(graphs, dto, lastPriority + 1);

        var newMainGraph = newGraphs[0];
        newGraphs.RemoveAt(0);

        newMainGraph.OperationGraphMainGroups = new List<OperationGraphGroup>();
        newGraphs.ForEach(g => newMainGraph.OperationGraphMainGroups.Add(new OperationGraphGroup { OperationGraphNext = g }));

        await _context.AddWithValidationsAndSaveAsync(newMainGraph, this);
        if (HasErrors) return 0;

        return newMainGraph.Id;
    }

    /// <summary>
    /// Удаление группы графиков
    /// </summary>
    /// <param name="mainGraphId">Id main графика в группе</param>
    /// <returns></returns>
    public async Task DeleteAsync(int mainGraphId)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, mainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        graph!.OperationGraphMainGroups!.ForEach(gu => OperationGraphValidations.GraphInWorkWithoutConfirmedDetails(gu.OperationGraphNext, this));
        if (HasErrors) return;

        _repository.Remove(graph);
        graph!.OperationGraphMainGroups!.ForEach(g => _repository.Remove(g.OperationGraphNext));

        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Удаленние графиков из группы с возможностью выборы типа удаления
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns></returns>
    public async Task DeleteGraphsAsync(DeleteGraphsInGroupDto dto)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, dto.MainGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        await OperationGraphRead.LoadUsersWithAccessAsync(_repository, mainGraph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, mainGraph, _dataMapper);

        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(mainGraph, this);
        if (HasErrors) return;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        // скидываем все графики в один список
        var localGraphs = new List<OperationGraph> { mainGraph! };
        localGraphs.AddRange(mainGraph!.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNext)!);

        if (localGraphs.Any(g => g.OperationGraphDetails!.Count > 0))
        {
            // очищаем всю информацию о графике (детали и их операции) и заполняем заново
            var graphGroupRepository = new OperationGraphGroupRepository();
            await graphGroupRepository.RecalculateGroupInfoAsync(localGraphs.Where(g => g.OperationGraphDetails!.Count > 0), _context, _dataMapper, this);
        }

        // очищаем все связи
        mainGraph.OperationGraphMainGroups!.ForEach(gg => gg!.OperationGraphNext!.OperationGraphNextGroups = null);
        mainGraph.OperationGraphMainGroups = null;

        // заносим в отдельный список графики на удаление и убираем их из локального списка
        var deletionGraphs = localGraphs.Where(g => dto.GraphsId.Contains(g.Id)).ToList();
        deletionGraphs.ForEach(g => localGraphs.Remove(g));

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        // запускаем событие удаление графиков
        var deleteEvent = new DeleteGraphsFromGroupEvent(dto.DeleteType, deletionGraphs);

        var deleteEventHandler = new LocalEventHandler(deleteEvent);
        await deleteEventHandler.ExecuteAsync(_context, _dataMapper, this);

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;

        // проверяем, остались ли графики в группе и состоит ли группа из 2+ графиков
        if (localGraphs.Any() && localGraphs.Count > 1)
        {
            // заново группируем оставшиеся графики
            var graphGroupBuilder = new OperationGraphGroupBuilder(localGraphs.First(), localGraphs.Skip(1).ToList());
            await graphGroupBuilder.GroupingAsync(_context, this);
            graphGroupBuilder.Build();

            await _context.SaveChangesWithValidationsAsync(this);
            if (HasErrors) return;
        }

        await transaction.CommitAsync();    
    }

    /// <summary>
    /// Получение группы графиков
    /// </summary>
    /// <param name="priority">Приоритет группы</param>
    /// <param name="userId">Id пользователя</param>
    /// <returns>Группа графиков</returns>
    public async Task<GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>?> GroupAsync(int priority, int userId)
    {
        var graphs = await new OperationGraphRequests(_context, _dataMapper).GroupAsync(priority, userId);
        if (graphs.IsNullOrEmpty())
        {
            AddErrors("Не удалось получить группу графиков");
            return null;
        }
        
        var mainGraph = graphs!.Single(g => g.IsMainGraph);

        var result = new GetAllOperationGraphDictionaryDto<GetAllOperationGraphDto>
        {
            Priority = mainGraph.Priority,
            IsGroup = graphs!.Count > 1,
            MainGraphId = mainGraph.OperationGraphId,
            Graphs = graphs
        };

        return result;
    }

    /// <summary>
    /// Получаем информацию о группе графиков
    /// </summary>
    /// <param name="filters">Фильтры выборки, где grahId - id main графика группы</param>
    /// <param name="userId">Id пользователя, который открывает график</param>
    /// <param name="graphService">Сервис операционных графиков</param>
    /// <returns>Список открытых графиков с их деталями и операциями деталей</returns>
    public async Task<OpenOperationGraphGroupDto?> OpenAsync(OpenOperationGraphFilters filters, int userId, IOperationGraphService graphService)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, filters.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);

        if (HasErrors) return null;

        var graphIds = new List<int> { mainGraph!.Id };
        graphIds.AddRange(mainGraph.OperationGraphMainGroups!.Select(mg => mg.OperationGraphNextId));

        var result = new OpenOperationGraphGroupDto
        {
            MainGraphId = filters.GraphId,
            IsStatusConfirmed = await OperationGraphRead.IsStatusConfirmedAsync(_context, filters.GraphId),
            Graphs = new List<GraphInfoDto>()
        };

        foreach (var graphId in graphIds)
        {
            filters.GraphId = graphId;
            
            var graphInfo = await graphService.OpenAsync(filters, userId);
            if (HasErrors) return null;

            result.Graphs.Add(graphInfo!.GraphInfo);
        }

        result.IsGroupConfirmed = result.Graphs.All(g => g.IsConfirmed);

        return result;
    }

    public void Dispose() => _context.Dispose();
}