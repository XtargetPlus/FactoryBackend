using AutoMapper;
using BizLayer.Builders.GraphBuilders;
using BizLayer.Repositories.DetailR;
using BizLayer.Repositories.GraphR;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using BizLayer.Repositories.GraphR.GraphDetailR;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.ProductInfo;
using DB.Model.StorageInfo.Graph;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Graph.Detail;
using Shared.Dto.Graph.Read.Open;

namespace ServiceLayer.Graphs.Services;

/// <summary>
/// 
/// </summary>
public class OperationGraphDetailService : ErrorsMapper, IOperationGraphDetailService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraphDetail> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphDetailService(IMapper dataMapper, DbApplicationContext context)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление детали в график
    /// </summary>
    /// <param name="dto">Информация для добавления. Usability указывать обязательно</param>
    /// <returns></returns>
    public async Task AddAsync(AddGraphDetailDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphDetailRead.LoadDetailsAsync(_context, graph, _dataMapper);

        OperationGraphValidations.GraphInWorkOrPausedWithoutConfirmedDetails(graph, this);
        if (HasErrors) return;

        var graphsDetails = graph!.OperationGraphDetails!.Where(d => d.TotalPlannedNumber.HasValue).ToList();
        foreach (var group in graph.OperationGraphMainGroups!)
        {
            graphsDetails.AddRange(group.OperationGraphNext!.OperationGraphDetails!.Where(d => d.TotalPlannedNumber.HasValue));
        }

        var newPositionNumber =  int.Parse(graph.OperationGraphDetails!.Last().DetailGraphNumberWithRepeats.Split('.')[0]) + 1;
        var compositionalDetails = new List<GetProductDetailsWithUsabilityDto> { new()
        {
            DetailId = dto.DetailId,
            PositionNumber = newPositionNumber.ToString(),
            Usability = dto.Usability
        } };
        compositionalDetails.AddRange(await new DetailRepository(this, _dataMapper).GetProductDetailsWithRepeatsAndUsabilityAsync(dto.DetailId, _context));
        for (var i = 1; i < compositionalDetails.Count; i++)
        {
            compositionalDetails[i].PositionNumber = compositionalDetails[0].PositionNumber + '.' + compositionalDetails[i].PositionNumber;
        }

        var detailRepository = new OperationGraphDetailRepository();
        var detailBuilder = new OperationGraphDetailBuilder(compositionalDetails);
        detailBuilder.CalculatePlannedNumber(graph.PlanCount);
        detailBuilder.CalculateNumber(graph.OperationGraphDetails!.Last().DetailGraphNumber);
        var newDetails = detailBuilder.Build();
        detailRepository.CalculateAfterAdding(graphsDetails, newDetails);

        var graphBuilder = new OperationGraphBuilder(graph);
        graphBuilder.SetDetailsForBuilder(newDetails);
        await graphBuilder.CalculateItemsInfoAsync(_context, _dataMapper, this);
        newDetails = graphBuilder.OperationGraphDetails!.Build();

        graph.OperationGraphDetails!.AddRange(newDetails);

        detailRepository.RecalculateNumberWithoutRepeats(graph.OperationGraphDetails);
        detailRepository.RecalculateLocalNumber(graph.OperationGraphDetails);

        if (HasErrors) return;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменение тех процесса детали графиков
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns></returns>
    public async Task ChangeTechProcessAsync(ChangeTechProcessDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, dto.GraphDetailId, this);
        await OperationGraphDetailItemRead.LoadDetailItemsAsync(_context, detail);
        if (HasErrors) return;

        detail!.OperationGraphDetailItems = null;

        // получаем операции тех процессов из main ветки
        var newItemsId = await TechProcessItemSimpleRead.GetMainIdsAsync(_context, detail.DetailId, dto.TechProcessId);

        detail.TechnologicalProcessId = dto.TechProcessId;
        detail.OperationGraphDetailItems = new List<OperationGraphDetailItem>();

        // TODO: получать сгд и поток
        // Временно
        detail.FinishedGoodsInventory = 0;
        detail.CountInStream = 0;

        detail.OperationGraphDetailItems.AddRange(newItemsId.Select((t, i) => new OperationGraphDetailItem
        {
            TechnologicalProcessItemId = t,
            OrdinalNumber = i + 1
        }));

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Подтверждение детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns></returns>
    public async Task ConfirmAsync(int graphDetailId)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, graphDetailId, this);
        if (HasErrors) return;

        detail!.IsConfirmed = true;

        // TODO: после подтверждения резервировать поток и сгд

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Снятие подтверждения детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns></returns>
    public async Task UnconfirmAsync(int graphDetailId)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, graphDetailId, this);
        if (HasErrors) return;

        detail!.IsConfirmed = false;

        // TODO: после снятия подтверждения снимать зарервированный поток и сгд

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Временный метод для добавления СГД к детали
    /// </summary>
    /// <param name="dto">Информация для добавления СГД</param>
    /// <returns></returns>
    public async Task AddFinishedGoodsInventoryAsync(AddFinishedGoodsInventoryDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, dto.GraphDetailId, this);
        if (HasErrors) return;

        if (detail!.FinishedGoodsInventory == dto.FinishedGoodsInventory) return;

        detail.FinishedGoodsInventory = dto.FinishedGoodsInventory;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Временный метод для добавления потока в деталь
    /// </summary>
    /// <param name="dto">Информация для добавления потока</param>
    /// <returns></returns>
    public async Task AddCountInStreamAsync(AddCountInStreamDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, dto.GraphDetailId, this);
        if (HasErrors) return;

        if (detail!.CountInStream == dto.CountInStream) return;

        detail.CountInStream = dto.CountInStream;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Скрытие или раскрытие детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <param name="visibility">Tru - расскрыть, False - скрыть</param>
    /// <returns></returns>
    public async Task HideOrUncoverAsync(int graphDetailId, bool visibility)
    {
        var detail = await OperationGraphDetailRead.WithoutFiltersAsync(_context, graphDetailId, this);
        if (HasErrors) return;

        detail!.IsVisible = visibility;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменение применяемости для детали графика
    /// </summary>
    /// <param name="dto">Информация для изменения применяемости</param>
    /// <returns></returns>
    public async Task ChangeUsabilityAsync(ChangeUsabilityDto dto)
    {
        var detail = await OperationGraphDetailRead.ByIdAsync(_repository, dto.GraphDetailId, this);
        await OperationGraphDetailRead.LoadMainGroupDetail(_context, detail, _dataMapper);
        if (HasErrors) return;

        var graphPlanCount = await OperationGraphRead.PlanCountAsync(_context, detail.OperationGraphId);
        var multipledUsability = await OperationGraphDetailRead.MultipledFathersUsabilitiesByDetailNumber(_context, detail.OperationGraphId, detail.DetailGraphNumberWithRepeats);

        var oldUsability = detail.Usability;
        detail.Usability = dto.Usability;

        var childDetails = await OperationGraphDetailRead.DetailsWithNumberStartByAsync(_context, detail);
        await OperationGraphDetailRead.LoadMainGroupDetail(_context, childDetails, _dataMapper);

        var details = new List<OperationGraphDetail> { detail };
        details.AddRange(childDetails!);

        var detailRepository = new OperationGraphDetailRepository();
        detailRepository.RecalculateAfterChangingUsability(details, oldUsability, graphPlanCount, multipledUsability, detail.Id);

        await _context.SaveChangesWithValidationsAsync(this);
        if (HasErrors) return;
    }

    /// <summary>
    /// Свап деталей графика
    /// </summary>
    /// <param name="dto">Информация для свапа, где TargetPositionNumber и SourcePositionNumber это LocalPositionNumber</param>
    /// <returns></returns>
    public async Task SwapAsync(SwapGraphDetailsDto dto)
    {
        var targetDetail = await OperationGraphDetailRead.ByPositionNumberToDisplayAsync(_repository, dto.GraphId, dto.TargetPositionNumber, this);
        var sourceDetail = await OperationGraphDetailRead.ByPositionNumberToDisplayAsync(_repository, dto.GraphId, dto.SourcePositionNumber, this);
        if (HasErrors) return;

        (targetDetail!.DetailGraphNumberWithoutRepeats, sourceDetail!.DetailGraphNumberWithoutRepeats) = (sourceDetail.DetailGraphNumberWithoutRepeats, targetDetail.DetailGraphNumberWithoutRepeats);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка детали между двумя деталями
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenAsync(InsertBetweenGraphDetailsDto dto)
    {
        var max = Math.Max(dto.TargetPositionNumber, dto.NewPositionNumber);
        var min = Math.Min(dto.TargetPositionNumber, dto.NewPositionNumber);

        var increment = dto.TargetPositionNumber > dto.NewPositionNumber ? 1 : -1;

        var details = await OperationGraphDetailRead.DetailByPositionNumberToDisplayRangeAsync(_context, dto.GraphId, min, max, this);
        if (HasErrors) return;

        details!.ForEach(d => d.DetailGraphNumberWithoutRepeats += increment);
        details.Single(d => d.DetailGraphNumberWithoutRepeats == dto.TargetPositionNumber + increment).DetailGraphNumberWithoutRepeats = dto.NewPositionNumber;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление детали графика
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns></returns>
    public async Task DeleteAsync(int graphDetailId)
    {
        var fatherDetail = await OperationGraphDetailRead.WithoutFiltersAsync(_context, graphDetailId, this);
        var children = await OperationGraphDetailRead.DetailsWithNumberStartByAsync(_context, fatherDetail);
        if (HasErrors) return;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var graphGroupIds = await OperationGraphRead.GroupIdsAsync(_context, fatherDetail!.OperationGraphId);

        var deleteDetails = new List<OperationGraphDetail> { fatherDetail };
        deleteDetails.AddRange(children!);

        await OperationGraphDetailRead.LoadMainGroupDetail(_context, deleteDetails, _dataMapper);
        await OperationGraphDetailRead.LoadNextGroupAsync(_context, deleteDetails, _dataMapper);

        var ignoreDetailIds = new OperationGraphDetailRepository().GroupByDetailId(deleteDetails);

        var detailRepository = new OperationGraphDetailRepository();
        
        var newMainDetails = await detailRepository.RecalculateMainInfoAfterDeleteAsync(_context, deleteDetails, ignoreDetailIds);

        if (newMainDetails.Count > 0)
        {
            var graphBuilder = new OperationGraphBuilder(new OperationGraph());
            graphBuilder.SetDetailsForBuilder(newMainDetails);
            await graphBuilder.CalculateItemsInfoAsync(_context, _dataMapper, this);
        }

        await _context.RemoveRangeWithValidationAndSaveAsync(deleteDetails, this);
        if (HasErrors) return;

        var details = await OperationGraphDetailRead.AllAsync(_context, fatherDetail.OperationGraphId);
        var groupDetails = await OperationGraphDetailRead.AllOrderByWithoutAsync(_context, graphGroupIds);

        if (details.Count > 0 && fatherDetail.DetailGraphNumber < details[^1].DetailGraphNumber)
        {
            var changingDetails = detailRepository.ToRecalculateNumberWithRepeatsAfterDelete(details);
            detailRepository.RecalculateNumberWithRepeatsAfterDelete(changingDetails);
            detailRepository.RecalculateNumberWithoutRepeats(groupDetails);
            detailRepository.RecalculateLocalNumberAfterDelete(changingDetails, deleteDetails.Count);

            await _context.SaveChangesWithValidationsAsync(this);
            if (HasErrors) return;
        }

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Получение детали по id
    /// </summary>
    /// <param name="graphDetailId">Id детали графика</param>
    /// <returns>Деталь графика или null</returns>
    public async Task<GraphDetailDto?> ByIdAsync(int graphDetailId)
    {
        var detail = await OperationGraphDetailItemRead.ByIdToGraphAsync(_context, graphDetailId, _dataMapper, this);
        if (HasErrors) return null;

        if (!detail!.TotalPlanCount.HasValue) return detail;
        
        var dict = new Dictionary<int, int> { { detail.GraphDetailId, detail.TechProcessId } };
        var itemBuilder = new OpenOperationGraphItemBuilder(dict, _context, _dataMapper, this);
        var items = await itemBuilder.BuildAsync();
        detail.ItemsInfo = items[detail.GraphDetailId];

        return detail;
    }

    public void Dispose() => _context.Dispose();
}