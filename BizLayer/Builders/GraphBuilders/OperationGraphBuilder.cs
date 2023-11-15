using AutoMapper;
using BizLayer.Repositories.DetailR;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using BizLayer.Repositories.TechnologicalProcessR;
using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Graph.CUD;
using Shared.Enums;

namespace BizLayer.Builders.GraphBuilders;

public class OperationGraphBuilder : BaseBuilder<OperationGraph>
{
    public OperationGraphDetailBuilder? OperationGraphDetails { get; private set; }

    private readonly OperationGraph _graph;

    public OperationGraphBuilder(AddGraphDto dto)
    {
        _graph = new OperationGraph()
        {
            SubdivisionId = dto.SubdivisionId,
            GraphDate = dto.GraphDate,
            ProductDetailId = dto.DetailId,
            PlanCount = dto.PlanCount,
            Note = dto.Note,
            StatusId = (int)GraphStatus.InWork,
        };
    }

    public OperationGraphBuilder(OperationGraph graph)
    {
        _graph = graph;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    public void SetOwner(int ownerId) => _graph.OwnerId = ownerId;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="priority"></param>
    public void SetPriority(int priority) => _graph.Priority = priority;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    public void SetDetailsForBuilder(List<GetProductDetailsWithUsabilityDto> dto) => OperationGraphDetails = new OperationGraphDetailBuilder(dto);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    public void SetDetailsForBuilder(List<OperationGraphDetail> details) => OperationGraphDetails = new OperationGraphDetailBuilder(details);

    /// <summary>
    /// 
    /// </summary>
    public void ClearDetailsInfo() =>  _graph.OperationGraphDetails = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="planCount"></param>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public async Task CalculateDetailsInfoAsync(int productId, float planCount, DbContext context, IMapper dataMapper, ErrorsMapper errors)
    {
        var detailRepository = new DetailRepository(errors, dataMapper);

        // Получаем детали графика в базовом виле
        var compositionalDetails = await detailRepository.GetProductDetailsWithRepeatsAndUsabilityAsync(productId, context);

        SetDetailsForBuilder(compositionalDetails);
        OperationGraphDetails!.CalculatePlannedNumber(planCount);  // Считаем план для каждой детали
        OperationGraphDetails.CalculateTotalPlannedNumber(); // считаем общий план с учетом всех повторов и заносим в впервые встреченные детали
        OperationGraphDetails.CalculateUsabilitySum(); // Считаем суммарную применяемость только для впервые встреченных деталей
        OperationGraphDetails.CalculateNumberWithoutRepeats(); // Считаем порядковый номер для отображения "Без повторов"
        OperationGraphDetails.CalculateNumber(); // Считаем порядковый номер для всего графика
        OperationGraphDetails.GroupRepeatingDetails(); // Группируем повторы
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public async Task CalculateItemsInfoAsync(DbContext context, IMapper dataMapper, ErrorsMapper errors)
    {
        var graphDetailItemRepository = new OperationGraphDetailItemRepository(errors, dataMapper);

        await OperationGraphDetails!.SetTechProcessIds(context);
        // Получаем главные выполненные тех процессы деталей, если они есть
        var detailsAndTechProcessesId = await TechProcessSimpleRead.GetCompletedMainByDetailIdAsync(
            context,
            OperationGraphDetails.DetailsIdWithoutRepeating.ToList(),
            errors
        );
        // получаем операции тех процессов из main ветки
        var techProcessAndItemsId = await TechProcessItemSimpleRead.GetItemsByTechProcessIdListAsync(
            context,
            detailsAndTechProcessesId.Select(t => t.Key).ToList(),
            errors
        );
        // генерируем операции деталей, заменяем тех процесс списком его операций
        var graphDetailsItems = graphDetailItemRepository.GenerateBaseGrapDetailsItems(detailsAndTechProcessesId, techProcessAndItemsId);

        OperationGraphDetails.SetItems(graphDetailsItems);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override OperationGraph Build()
    {
        if (OperationGraphDetails is not null)
            _graph.OperationGraphDetails = OperationGraphDetails.Build();

        return _graph;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<OperationGraph> BuildAsync()
    {
        if (OperationGraphDetails is not null)
            _graph.OperationGraphDetails = await OperationGraphDetails.BuildAsync();

        return _graph;
    }
}