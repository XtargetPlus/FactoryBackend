using AutoMapper;
using BizLayer.Repositories.GraphR.Variables;
using DatabaseLayer.Helper;

namespace BizLayer.Repositories.GraphR.GraphDetailItemR;

public class OperationGraphDetailItemRepository
{
    private readonly ErrorsMapper _mapper;
    private readonly IMapper _dataMapper;

    public OperationGraphDetailItemRepository(ErrorsMapper mapper, IMapper dataMapper) =>
        (_mapper, _dataMapper) = (mapper, dataMapper);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detailsAndTechProcessesId"></param>
    /// <param name="techProcessAndItemsId"></param>
    /// <returns></returns>
    public List<OperationGraphDetailItemAddingInfo> GenerateBaseGrapDetailsItems(
        Dictionary<int, int> detailsAndTechProcessesId,
        Dictionary<int, List<int>> techProcessAndItemsId)
    {
        var graphDetailsItems = new List<OperationGraphDetailItemAddingInfo>();

        var union = detailsAndTechProcessesId
            .Join(
                techProcessAndItemsId,
                d => d.Key,
                t => t.Key,
                (d, t) => new
                {
                    DetailId = d.Value,
                    TechProcessItems = t.Value
                }
            );

        foreach (var item in union)
        {
            graphDetailsItems
                .AddRange(
                    item.TechProcessItems.Select(
                        (t, i) => new OperationGraphDetailItemAddingInfo()
                        {
                            DetailId = item.DetailId,
                            TechnologicalProcessItemId = t,
                            OrdinalNumber = i + 1,
                        }
                    )
                );
        }

        return graphDetailsItems;
    }
}