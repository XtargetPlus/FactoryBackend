using AutoMapper;
using BizLayer.Repositories.GraphR.GraphDetailItemR;
using BizLayer.Repositories.TechnologicalProcessR;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using DatabaseLayer.DbRequests.GraphToDb;
using DatabaseLayer.Helper;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Read.Open;

namespace BizLayer.Builders.GraphBuilders;

public class OpenOperationGraphItemBuilder : BaseBuilder<Dictionary<int, GraphDetailItemHigherDto>>
{
    private readonly Dictionary<int, GraphDetailItemHigherDto> _resultItems;
    private readonly Dictionary<int, int> _detailIds;
    private readonly DbContext _context;
    private readonly IMapper _dataMapper;
    private readonly ErrorsMapper _mapper;

    public OpenOperationGraphItemBuilder(Dictionary<int, int> detailIds, DbContext context, IMapper dataMapper, ErrorsMapper mapper)
    {
        _detailIds = detailIds;
        _context = context;
        _dataMapper = dataMapper;
        _mapper = mapper;
        _resultItems = new Dictionary<int, GraphDetailItemHigherDto>();

        foreach (var detailId in detailIds)
            _resultItems[detailId.Key] = new GraphDetailItemHigherDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override Dictionary<int, GraphDetailItemHigherDto> Build() => _resultItems;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<Dictionary<int, GraphDetailItemHigherDto>> BuildAsync()
    {
        var baseItems = await GetBaseItems();
        if (baseItems is null)
        {
            _mapper.AddErrors("Не удалось получить информацию об операциях");
            return _resultItems;
        }

        foreach (var baseItem in baseItems)
        {
            _resultItems[baseItem.Key].GroupItems = await GetGroupItems(baseItem.Value);
            
            _resultItems[baseItem.Key].PossibleToAddToEnd = await CheckPossibleToAddToEnd(baseItem.Value.Where(i => i.Priority % 5 == 0), _detailIds[baseItem.Key]);
        }

        return _resultItems;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<Dictionary<int, List<ReadGraphDetailItemDto>>?> GetBaseItems() =>
        await new OperationGraphDetailItemRequests(_context, _dataMapper).ItemsForOpenAsync(_detailIds.Keys.ToList());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseItems"></param>
    /// <returns></returns>
    private async Task<List<GraphDetailItemGroupDto>> GetGroupItems(IEnumerable<ReadGraphDetailItemDto> baseItems)
    {
        var result = new List<GraphDetailItemGroupDto>();

        foreach (var item in baseItems.GroupBy(i => i.Priority))
        {
            var localItem = new GraphDetailItemGroupDto
            {
                Items = new List<ReadGraphDetailItemDto>(item)
            };

            if (item.Key % 5 == 0)
            {
                localItem.PossibleToAddToEnd = false;
                localItem.IsHaveBranches = await TechProcessItemSimpleRead.IsHaveBranchesAsync(_context, item.Single().ItemId);
            }
            else
            {
                var mainTechProcessItemId = await TechProcessItemSimpleRead.GetMainIdAsync(_context, item.First().ItemId);
                if (mainTechProcessItemId is null)
                {
                    _mapper.AddErrors("Не удалось получить id main операции тех процесса");
                    return result;
                }

                localItem.PossibleToAddToEnd = await OperationGraphDetailItemRead.BranchPossibleToAddToEndAsync(item.Count(), item.Key, mainTechProcessItemId.Value, _context);
                localItem.IsHaveBranches = true;
            }

            result.Add(localItem);
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mainItems"></param>
    /// <param name="techProcessId"></param>
    /// <returns></returns>
    private async Task<bool> CheckPossibleToAddToEnd(IEnumerable<ReadGraphDetailItemDto> mainItems, int techProcessId) =>
        await TechProcessItemSimpleRead.GetMainsCountAsync(_context, techProcessId) != mainItems.Count();
}