using BizLayer.Repositories.GraphR;
using BizLayer.Repositories.GraphR.GraphDetailR;
using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;

namespace BizLayer.Builders.GraphBuilders;

public class OperationGraphGroupBuilder : BaseBuilder<OperationGraph>
{
    private readonly OperationGraph _mainGraph;
    private readonly List<OperationGraph> _groupGraphs;

    public OperationGraphGroupBuilder(OperationGraph mainGraph, List<OperationGraph> groupGraphs)
    {
        _mainGraph = mainGraph;
        _groupGraphs = groupGraphs;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public async Task GroupingAsync(DbContext context, ErrorsMapper errors)
    {
        var graphGroupRepository = new OperationGraphGroupRepository();
        var graphDetailRepository = new OperationGraphDetailRepository();
        
        // зависимые детали
        var slaveDetails = await graphDetailRepository.SlaveDetailsInGroupGraphs(context, _mainGraph, _groupGraphs);

        var localGraphs = _groupGraphs.Where(g => g.OperationGraphDetails!.Count > 0).ToList();
        // локальный main, чтобы искать повторы в графиках с индексами currentGraphIndex < otherGraphIndex
        var localGraph = _mainGraph;

        // порядковый номер без повторов
        var numberWithoutRepeats = graphDetailRepository.LastNumberWithoutRepeats(_mainGraph);

        // создаем группу графиков
        graphGroupRepository.AddGraphsInGroup(_mainGraph, _groupGraphs);

        while (localGraphs.Count > 0)
        {
            // получаем повторы в других графиках
            var repeatsDetailsInOtherGraphs = graphDetailRepository.RepeatsDetailsInOtherGraphs(
                localGraph.OperationGraphDetails!,
                localGraphs
            );

            if (repeatsDetailsInOtherGraphs.Count > 0)
            {
                // идем по деталям из локального main
                graphGroupRepository.AddDetailsInGroup(localGraph, repeatsDetailsInOtherGraphs, slaveDetails);
                // получаем список графиков, в которых есть повторы
                var repeatsGraphs = graphGroupRepository.GraphsWithRepeats(repeatsDetailsInOtherGraphs);
                // проходимся только по графикам с повторами
                graphGroupRepository.ClearRepetitions(localGraphs, repeatsDetailsInOtherGraphs, repeatsGraphs);
            }

            // приводим все номера к единому порядку
            numberWithoutRepeats = graphGroupRepository.AddNewNumberWithoutRepeats(localGraphs[0], numberWithoutRepeats);

            // переходим на шаг вправо
            localGraph = localGraphs[0];

            if (localGraphs.Remove(localGraphs[0])) continue;

            errors.AddErrors("Ошибка в вычислениях");
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override OperationGraph Build() => _mainGraph;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<OperationGraph> BuildAsync() => _mainGraph;
}