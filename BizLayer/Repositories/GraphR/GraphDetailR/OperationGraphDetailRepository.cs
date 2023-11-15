using BizLayer.Repositories.GraphR.Variables;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;

namespace BizLayer.Repositories.GraphR.GraphDetailR;

public class OperationGraphDetailRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="operationDetails"></param>
    /// <param name="newPlanCount"></param>
    /// <returns></returns>
    public Dictionary<int, float> RecalculatePlannedNumber(Dictionary<int, float> operationDetails, float newPlanCount)
    {
        var localOperationDetails = operationDetails.ToDictionary(d => d.Key, d => d.Value);

        foreach (var localOperationDetail in localOperationDetails)
            localOperationDetails[localOperationDetail.Key] = localOperationDetail.Value * newPlanCount;

        return localOperationDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    /// <param name="oldUsability"></param>
    /// <param name="planCount"></param>
    /// <param name="multipledUsability"></param>
    /// <param name="ignoreId"></param>
    public void RecalculateAfterChangingUsability(List<OperationGraphDetail> details, float oldUsability, float planCount, float multipledUsability, int ignoreId)
    {
        oldUsability *= multipledUsability; 

        foreach (var detail in details)
        {
            multipledUsability *= detail.Usability;
            if (detail.Id != ignoreId)
                oldUsability *= detail.Usability;

            detail.UsabilityWithFathers = multipledUsability;
            detail.PlannedNumber = multipledUsability * planCount;

            var mainDetail = detail.TotalPlannedNumber.HasValue
                ? detail
                : detail.OperationGraphNextDetails![0].OperationGraphMainDetail;

            mainDetail!.TotalPlannedNumber -= oldUsability * planCount;
            mainDetail.TotalPlannedNumber += multipledUsability * planCount;

            mainDetail.UsabilitySum -= oldUsability;
            mainDetail.UsabilitySum += multipledUsability;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    /// <param name="ignoreDetailIds"></param>
    /// <returns></returns>
    public async Task<List<OperationGraphDetail>> RecalculateMainInfoAfterDeleteAsync(DbContext context, List<OperationGraphDetail> details, Dictionary<int, List<int>> ignoreDetailIds)
    {
        var newMainDetails = new List<OperationGraphDetail>();

        foreach (var detail in details)
        {
            if (!detail.TotalPlannedNumber.HasValue)
            {
                var mainDetailId = detail.OperationGraphNextDetails!.Single().OperationGraphMainDetail!.Id;
                var mainDetail = details.Any(d => d.Id == mainDetailId) 
                    ? newMainDetails.First(d => d.Id == mainDetailId) 
                    : detail.OperationGraphNextDetails!.Single().OperationGraphMainDetail!;

                mainDetail.TotalPlannedNumber -= detail.PlannedNumber;
                mainDetail.UsabilitySum -= detail.UsabilityWithFathers;
                continue;
            }

            var detailId = ignoreDetailIds.Keys.First(k => k == detail.DetailId);
            var newMainDetail = detail.OperationGraphMainDetails!
                .Where(md => !ignoreDetailIds[detailId].Contains(md.OperationGraphNextDetailId))
                .Select(md => md.OperationGraphNextDetail)
                .FirstOrDefault();
            // var newMainDetail = await OperationGraphDetailRead.NewMainAsync(context, detailId, ignoreDetailIds[detailId], graphGroupIds);
            if (newMainDetail is null) continue;

            newMainDetail.TotalPlannedNumber = detail.TotalPlannedNumber - detail.PlannedNumber;
            newMainDetail.UsabilitySum = detail.UsabilitySum - detail.UsabilityWithFathers;
            newMainDetail.OperationGraphMainDetails = detail.OperationGraphMainDetails!
                .Where(d => d.OperationGraphNextDetailId != newMainDetail.Id)
                .Select(d => new OperationGraphDetailGroup
                {
                    OperationGraphNextDetail = d.OperationGraphNextDetail
                })
                .ToList();
            newMainDetails.Add(newMainDetail);
        }

        return newMainDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public List<List<OperationGraphDetail>> ToRecalculateNumberWithRepeatsAfterDelete(List<OperationGraphDetail> details)
    {
        var groupDetails = new List<List<OperationGraphDetail>> { details.Where(d => !d.DetailGraphNumberWithRepeats.Contains('.')).ToList() };
        foreach (var detail in details.Where(d => d.DetailGraphNumberWithRepeats.Contains('.')))
        {
            var number = string.Join("", detail.DetailGraphNumberWithRepeats.Split('.'));
            var group = groupDetails.FirstOrDefault(g => 
                g.First().DetailGraphNumberWithRepeats.Split('.').Length - 1 == number.Length
                && g.First().DetailGraphNumberWithRepeats.StartsWith(number));
            
            if (!group.IsNullOrEmpty()) group!.Add(detail);
            else groupDetails.Add(new List<OperationGraphDetail> { detail });
        }

        var root = new List<OperationGraphDetail>();
        foreach (var group in groupDetails)
        {
            if (group[0].DetailGraphNumberWithRepeats[^1] != 1)
            {
                root.AddRange(group);
                continue;
            }
            for (var i = 0; i < group.Count - 1; i++)
            {
                var left = int.Parse(string.Join("", group[i + 1].DetailGraphNumberWithRepeats));
                var right = int.Parse(string.Join("", group[i].DetailGraphNumberWithRepeats.Split('.')));
                if (left - right <= 1) continue;
                
                root = group.ToArray()[(i + 1)..].ToList();
                break;
            }

            if (root.Count == 0) continue;
            break;
        }

        var result = new List<List<OperationGraphDetail>> { root };

        foreach (var node in root)
        {
            var clearedGroups = groupDetails.Where(g => !g.Contains(node));
            
            result.AddRange(clearedGroups.Where(g => 
                g.All(x => x.DetailGraphNumberWithRepeats.StartsWith(node.DetailGraphNumberWithRepeats))));
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupDetails"></param>
    public void RecalculateNumberWithRepeatsAfterDelete(List<List<OperationGraphDetail>> groupDetails)
    {
        foreach (var group in groupDetails)
        {
            var x = 1;
            for (var i = 1; i < group[0].DetailGraphNumberWithRepeats.Split('.').Length; i++)
            {
                x *= 10;
            }

            foreach (var detail in group)
            {
                var oldNumber = int.Parse(string.Join("", detail.DetailGraphNumberWithRepeats.Split('.')));
                oldNumber -= x;
                var newNumber = string.Join('.', oldNumber.ToString().Split(""));
                detail.DetailGraphNumberWithRepeats = newNumber;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    public void RecalculateNumberWithoutRepeats(List<OperationGraphDetail> details)
    {
        var i = 1;
        foreach (var detail in details.Where(d => d.TotalPlannedNumber.HasValue))
        {
            detail.DetailGraphNumberWithoutRepeats = i;
            i++;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupDetails"></param>
    /// <param name="allDeletedDetails"></param>
    public void RecalculateLocalNumberAfterDelete(List<List<OperationGraphDetail>> groupDetails, int allDeletedDetails)
    {
        foreach (var detail in groupDetails.SelectMany(group => group))
        {
            detail.DetailGraphNumber -= allDeletedDetails;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    public void RecalculateLocalNumber(List<OperationGraphDetail> details)
    {
        for (var i = 0; i < details.Count; i++)
        {
            details[i].DetailGraphNumber = i + 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentDetails"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    public float CalculateUsabilityWithFathers(List<OperationGraphDetail> parentDetails, OperationGraphDetail detail)
    {
        var usability = detail.Usability;

        for (var i = detail.DetailGraphNumberWithRepeats.Split(".").Length - 1; i > 0; i--)
        {
            usability *= parentDetails
                .First(d => d.DetailGraphNumberWithRepeats == string.Join('.', detail.DetailGraphNumberWithRepeats.Split(".")[..^i]))
                .Usability;
        }

        return usability;
    }

    /// <summary>
    /// Указываем в какую деталь, из какого графика, нужно добавить повторы
    /// </summary>
    /// <param name="mainDetails"></param>
    /// <param name="otherGraphs"></param>
    /// <returns></returns>
    public Dictionary<int, List<GraphDetailInfo>> RepeatsDetailsInOtherGraphs(
        List<OperationGraphDetail> mainDetails,
        List<OperationGraph> otherGraphs)
    {
        var result = new Dictionary<int, List<GraphDetailInfo>>();
        //var result = new Dictionary<int, Dictionary<int, GraphDetailInfo>>();

        foreach (var mainDetail in mainDetails.Where(d => d.TotalPlannedNumber.HasValue))
        {
            result[mainDetail.Id] = otherGraphs
                .Select(g => g.OperationGraphDetails!
                    .Where(gd => gd.DetailId == mainDetail.DetailId && gd.TotalPlannedNumber.HasValue)
                    .Select(gd => new GraphDetailInfo
                    {
                        GraphDetailId = gd.Id,
                        GraphId = gd.OperationGraphId,
                        TotalPlanCount = (float)gd.TotalPlannedNumber!,
                        UsabilityCount = (float)gd.UsabilitySum!
                    })
                    .FirstOrDefault()
                )
                .Where(r => r is not null)
                .ToList()!;
        }

        result = result.Where(r => r.Value.Count > 0).ToDictionary(k => k.Key, v => v.Value);

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    public int LastNumberWithoutRepeats(OperationGraph graph)
    {
        var maxGraphNumber = 0;
        if (graph.OperationGraphDetails!.Count > 0)
            maxGraphNumber = graph.OperationGraphDetails!.Select(gd => gd.DetailGraphNumberWithoutRepeats).Max();

        if (!graph.OperationGraphMainGroups?.Any() ?? true)
            return maxGraphNumber;

        var numbers = new List<int> { maxGraphNumber };
        var details = new List<OperationGraphDetail>();
        foreach (var group in graph.OperationGraphMainGroups)        
        {
            details.AddRange(group.OperationGraphNext!.OperationGraphDetails!);
        }
        if (details.Count > 0) 
            numbers.Add(details.Select(d => d.DetailGraphNumberWithoutRepeats).Max());

        return numbers.Max();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public Dictionary<int, List<int>> GroupByDetailId(List<OperationGraphDetail> details)
    {
        var result = new Dictionary<int, List<int>>();

        foreach (var detail in details.Where(detail => !result.ContainsKey(detail.DetailId)))
        {
            result.Add(detail.DetailId, details.Where(d => d.DetailId == detail.DetailId).Select(d => d.Id).ToList());
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mainGraph"></param>
    /// <param name="slaveGraphs"></param>
    /// <returns></returns>
    public async Task<List<OperationGraphDetail>> SlaveDetailsInGroupGraphs(DbContext context, OperationGraph mainGraph, List<OperationGraph> slaveGraphs)
    {
        var slaveDetails = new List<OperationGraphDetail>();

        var localGraphs = new List<OperationGraph> { mainGraph };
        localGraphs.AddRange(slaveGraphs);

        // загружаем группы деталей и заносим в отдельный список зависимые детали
        foreach (var graph in localGraphs)
        {
            await OperationGraphDetailRead.LoadMainGroupDetail(context, graph.OperationGraphDetails, null);
            foreach (var detail in graph.OperationGraphDetails!.Where(d => !d.OperationGraphNextDetails.IsNullOrEmpty()))
            {
                detail.OperationGraphNextDetails = null;
            }
            foreach (var detail in graph.OperationGraphDetails!.Where(d => !d.OperationGraphMainDetails.IsNullOrEmpty()))
            {
                slaveDetails.AddRange(detail.OperationGraphMainDetails!.Select(md => md.OperationGraphNextDetail!));
            }
        }

        return slaveDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphsDetails"></param>
    /// <param name="newDetails"></param>
    /// <returns></returns>
    public void CalculateAfterAdding(List<OperationGraphDetail> graphsDetails, List<OperationGraphDetail> newDetails)
    {
        var ignoredDetails = new List<int>();
        foreach (var detail in graphsDetails)
        {
            var similarDetails = newDetails.Where(d => d.DetailId == detail.DetailId).ToList();
            detail.TotalPlannedNumber += similarDetails.Select(d => d.TotalPlannedNumber).Sum();
            detail.UsabilitySum += similarDetails.Select(d => d.UsabilityWithFathers).Sum();
            if (similarDetails.Count > 0)
            {
                detail.OperationGraphMainDetails!.AddRange(similarDetails.Select(d => new OperationGraphDetailGroup
                {
                    OperationGraphNextDetail = d
                }));
            }

            ignoredDetails.Add(detail.DetailId);
        }

        foreach (var detail in newDetails.Where(d => !ignoredDetails.Contains(d.DetailId)))
        {
            var similarDetails = newDetails.Where(d => d.DetailId == detail.DetailId).ToList();
            detail.TotalPlannedNumber = similarDetails.Select(d => d.PlannedNumber).Sum();
            detail.UsabilitySum = similarDetails.Select(d => d.UsabilityWithFathers).Sum();
            if (similarDetails.Count > 0)
            {
                detail.OperationGraphMainDetails = new List<OperationGraphDetailGroup>();
                detail.OperationGraphMainDetails!.AddRange(similarDetails.Select(d => new OperationGraphDetailGroup
                {
                    OperationGraphNextDetail = d
                }));
            }

            ignoredDetails.Add(detail.DetailId);
        }
    }
}