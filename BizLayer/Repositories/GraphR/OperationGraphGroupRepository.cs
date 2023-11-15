using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using BizLayer.Builders.GraphBuilders;
using BizLayer.Repositories.GraphR.Variables;
using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.CUD;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR;

public class OperationGraphGroupRepository
{
    /// <summary>
    /// Занесение графиков в группу
    /// </summary>
    /// <param name="mainGraph">Main график</param>
    /// <param name="graphs">Графики, которые заносятся в main</param>
    public void AddGraphsInGroup(OperationGraph mainGraph, IEnumerable<OperationGraph> graphs)
    {
        mainGraph.OperationGraphMainGroups ??= new List<OperationGraphGroup>();
        mainGraph.OperationGraphMainGroups!.AddRange(graphs.Select(og => new OperationGraphGroup
        {
            OperationGraphNext = og,
        }));

        mainGraph.OperationGraphMainGroups.ForEach(g => g.OperationGraphNext!.Priority = mainGraph.Priority);
    }

    /// <summary>
    /// Добавление деталей в группу
    /// </summary>
    /// <param name="graph">График, для деталей которых будут добавляться в группу другие детали</param>
    /// <param name="repeats">Повторяющиеся детали, где Key - mainDetailId, Value - список деталей группы</param>
    public void AddDetailsInGroup(OperationGraph graph, Dictionary<int, List<GraphDetailInfo>> repeats, List<OperationGraphDetail> slaveDetails)
    {
        foreach (var detail in graph.OperationGraphDetails!.Where(d => repeats.Keys.Contains(d.Id)))
        {
            // суммируем общий план и суммарную применяемость только в уникальную деталь из main
            detail.TotalPlannedNumber += repeats
                .Where(d => d.Key == detail.Id)
                .Select(d => d.Value
                    .Select(rd => rd.TotalPlanCount)
                    .Sum())
                .Sum();
            detail.UsabilitySum += repeats
                .Where(d => d.Key == detail.Id)
                .Select(d => d.Value
                    .Select(rd => rd.UsabilityCount)
                    .Sum())
                .Sum();

            // создаем группы деталей графиков
            detail.OperationGraphMainDetails = new List<OperationGraphDetailGroup>();
            detail.OperationGraphMainDetails
                .AddRange(repeats
                    .FirstOrDefault(d => d.Key == detail.Id)
                    .Value.Select(rd => new OperationGraphDetailGroup
                    {
                        OperationGraphMainDetailId = detail.Id,
                        OperationGraphNextDetailId = rd.GraphDetailId
                    }));
            detail.OperationGraphMainDetails.AddRange(slaveDetails.Where(d => d.DetailId == detail.DetailId).Select(d => new OperationGraphDetailGroup
            {
                OperationGraphNextDetail = d
            }));
        }
    }

    /// <summary>
    /// Получаем список графиков, в которых есть повторы
    /// </summary>
    /// <param name="repeats">Повторяющиеся детали, где Key - mainDetailId, Value - список деталей группы</param>
    /// <returns>Список GraphId, в которых есть повторы</returns>
    public List<int> GraphsWithRepeats(Dictionary<int, List<GraphDetailInfo>> repeats)
    {
        var result = new List<int>();
        foreach (var repeatDetail in repeats)
        {
            repeatDetail.Value.ForEach(k =>
            {
                if (!result.Contains(k.GraphId))
                    result.Add(k.GraphId);
            });
        }

        return result;
    }

    /// <summary>
    /// Очищаем данные в повторяющихся деталях
    /// </summary>
    /// <param name="graphs">Графики, в которых будут очищаться повторы</param>
    /// <param name="repeatsDetails">Повторяющиеся детали, где Key - mainDetailId, Value - список деталей группы</param>
    /// <param name="repeatsGraphs">Повторяющиеся графики</param>
    public void ClearRepetitions(IEnumerable<OperationGraph> graphs, Dictionary<int, List<GraphDetailInfo>> repeatsDetails, List<int> repeatsGraphs)
    {
        foreach (var graph in graphs.Where(g => repeatsGraphs.Contains(g.Id)))
        {
            var details = graph.OperationGraphDetails;
            // получаем деталь с графиком, которая является повтором
            foreach (var detail in details!.Where(d => repeatsDetails.Values
                         .FirstOrDefault(v => v.FirstOrDefault(rv => rv.GraphDetailId == d.Id) is not null) is not null))
            {
                detail.OperationGraphDetailItems = null;
                detail.TotalPlannedNumber = null;
                detail.UsabilitySum = null;
                detail.DetailGraphNumberWithoutRepeats = 0;
                detail.OperationGraphMainDetails = null;
            }
        }
    }

    /// <summary>
    /// Присваем деталям графика новый порядковый номер без в форме отображения без повторов
    /// </summary>
    /// <param name="graph">График, для деталей которых будет присвоен новый порядковый номер</param>
    /// <param name="number">Последний присовенный порядковый номер, отсчет начнется от number + 1</param>
    /// <returns>Последний присовенный порядковый номер</returns>
    public int AddNewNumberWithoutRepeats(OperationGraph graph, int number)
    {
        foreach (var detail in graph.OperationGraphDetails!.Where(gd => gd.DetailGraphNumberWithoutRepeats != 0))
        {
            detail.DetailGraphNumberWithoutRepeats = ++number;
        }

        return number;
    }

    /// <summary>
    /// Пересчет информации графиков (детали и их операции)
    /// </summary>
    /// <param name="graphs">Список графиков</param>
    /// <param name="context">Контекст баз данных</param>
    /// <param name="dataMapper">Маппер данных</param>
    /// <param name="errorsMapper">Маппер ошибок</param>
    /// <returns>Требуется проверка на наличие ошибок</returns>
    public async Task RecalculateGroupInfoAsync(IEnumerable<OperationGraph> graphs, DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        foreach (var graph in graphs)
        {
            var graphBuilder = new OperationGraphBuilder(graph!);

            // очищаем информацию о деталях
            graphBuilder.ClearDetailsInfo();

            // заново генерируем детали и их операции
            await graphBuilder.CalculateDetailsInfoAsync(graph.ProductDetailId!.Value, graph.PlanCount, context, dataMapper, errorsMapper);
            await graphBuilder.CalculateItemsInfoAsync(context, dataMapper, errorsMapper);

            graphBuilder.Build();
        }
    }

    /// <summary>
    /// Копирование графиков (подходит для группы и не группы)
    /// </summary>
    /// <param name="graphs">Список графиков</param>
    /// <param name="dto">Новая информация для копии</param>
    /// <param name="priority">Новый приоритет</param>
    /// <returns>Скопированный список графиков</returns>
    public List<OperationGraph> Copy(List<OperationGraph> graphs, CopyGraphDto dto, int priority)
    {
        var newGraphs = new List<OperationGraph>();

        foreach (var graph in graphs)
        {
            // создаем новый график
            var newGraph = new OperationGraph
            {
                GraphDate = dto.GraphDate,
                OwnerId = graph.OwnerId,
                ProductDetailId = graph.ProductDetailId,
                Priority = priority,
                PlanCount = dto.PlanCount,
                SubdivisionId = dto.SubdivisionId,
                IsConfirmed = false,
                ConfigrmingId = null,
                Note = dto.Note,
                StatusId = (int)GraphStatus.InWork,
                OperationGraphDetails = new List<OperationGraphDetail>(),
            };
            newGraphs.Add(newGraph);

            // присваиваем новые детали
            graph.OperationGraphDetails!.ForEach(d => newGraph.OperationGraphDetails.Add(new OperationGraphDetail
            {
                DetailId = d.DetailId,
                DetailGraphNumber = d.DetailGraphNumber,
                DetailGraphNumberWithRepeats = d.DetailGraphNumberWithRepeats,
                DetailGraphNumberWithoutRepeats = d.DetailGraphNumberWithoutRepeats,
                PlannedNumber = d.PlannedNumber,
                TotalPlannedNumber = d.TotalPlannedNumber,
                Usability = d.Usability,
                UsabilitySum = d.UsabilitySum,
                TechnologicalProcessId = d.TechnologicalProcessId,
                Note = d.Note,
                OperationGraphDetailItems = new List<OperationGraphDetailItem>()
            }));

            for (var i = 0; i < newGraph.OperationGraphDetails.Count; i++)
            {
                // для каждой детали копируем операции
                graph.OperationGraphDetails![i].OperationGraphDetailItems!.ForEach(item => newGraph.OperationGraphDetails[i].OperationGraphDetailItems!.Add(new OperationGraphDetailItem
                {
                    OrdinalNumber = item.OrdinalNumber,
                    TechnologicalProcessItemId = item.TechnologicalProcessItemId,
                    Note = item.Note,
                }));

                if (newGraph.OperationGraphDetails[i].TotalPlannedNumber.HasValue) continue;

                // учитываем, что детали могут быть в группах и добавляем деталь в группу, если у нее нет TotalPlannedNumber 
                var detail = newGraphs
                    .First(g => g.OperationGraphDetails!.Any(d => d.DetailId == newGraph.OperationGraphDetails[i].DetailId && d.TotalPlannedNumber.HasValue))
                    .OperationGraphDetails!
                    .First(d => d.DetailId == newGraph.OperationGraphDetails[i].DetailId && d.TotalPlannedNumber.HasValue);

                detail.OperationGraphMainDetails ??= new List<OperationGraphDetailGroup>();
                detail.OperationGraphMainDetails.Add(new OperationGraphDetailGroup { OperationGraphNextDetail = newGraph.OperationGraphDetails[i] });
            }
        }

        return newGraphs;
    }
}