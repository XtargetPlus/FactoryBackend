using AutoMapper;
using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;

namespace BizLayer.Events.Graphs;

public class ChangeGraphInGroupEvent : IEvent
{
    private readonly float _newPlanCount;
    private readonly float _oldPlanCount;

    private List<OperationGraphDetail> _mainDetails;
    private Dictionary<int, OperationGraphDetail> _groupDetails;

    public ChangeGraphInGroupEvent(float oldPlanCount, float newPlanCount)
    {
        _newPlanCount = newPlanCount;
        _oldPlanCount = oldPlanCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    public void SetMainDetails(List<OperationGraphDetail> details) => _mainDetails = details;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupDetails"></param>
    public void SetGroupDetails(Dictionary<int, OperationGraphDetail> groupDetails) => _groupDetails = groupDetails;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errorsMapper"></param>
    public void Execute(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        RecalculatePlanCountForMainDetails();
        RecalculateTotalPlanCountForMainDetails();
        RecalculateTotalPlanCountForGroupDetails();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dataMapper"></param>
    /// <param name="errorsMapper"></param>
    /// <returns></returns>
    public async Task ExecuteAsync(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper) { }

    /// <summary>
    /// 
    /// </summary>
    private void RecalculateTotalPlanCountForGroupDetails()
    {
        foreach (var detail in _groupDetails)
        {
            detail.Value.TotalPlannedNumber = detail.Value.TotalPlannedNumber - _mainDetails
                                            .Where(d => d.DetailId == detail.Key)
                                            .Select(d => d.UsabilityWithFathers * _oldPlanCount).Sum()
                                        + _mainDetails
                                            .Where(d => d.DetailId == detail.Key)
                                            .Select(d => d.PlannedNumber).Sum();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RecalculatePlanCountForMainDetails()
    {
        _mainDetails.ForEach(d => d.PlannedNumber = d.UsabilityWithFathers * _newPlanCount);
    }

    /// <summary>
    /// 
    /// </summary>
    private void RecalculateTotalPlanCountForMainDetails()
    {
        var impuritiesTotalPlanCount = _mainDetails
            .Where(d => d.TotalPlannedNumber.HasValue)
            .Select(d => new
            {
                Key = d.DetailId,
                Value = d.TotalPlannedNumber!.Value - d.UsabilitySum!.Value * _oldPlanCount,
            })
            .ToDictionary(k => k.Key, v => v.Value);

        foreach (var detail in _mainDetails.Where(d => d.TotalPlannedNumber.HasValue))
        {
            detail.TotalPlannedNumber = detail.UsabilitySum!.Value * _newPlanCount + impuritiesTotalPlanCount[detail.DetailId];
        }
    }
}