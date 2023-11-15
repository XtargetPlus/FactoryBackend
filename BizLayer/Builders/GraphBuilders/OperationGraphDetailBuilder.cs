using BizLayer.Repositories.GraphR.GraphDetailR;
using BizLayer.Repositories.GraphR.Variables;
using DB.Model.StorageInfo.Graph;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail.DetailChild;

namespace BizLayer.Builders.GraphBuilders;

public class OperationGraphDetailBuilder : BaseBuilder<List<OperationGraphDetail>>
{
    private List<OperationGraphDetail> _operationGraphDetails;

    public List<int> DetailsIdWithoutRepeating
    {
        get
        {
            return _operationGraphDetails.Where(d => d.TotalPlannedNumber.HasValue).Select(d => d.DetailId).ToList();
        }
    }

    public OperationGraphDetailBuilder(IEnumerable<GetProductDetailsWithUsabilityDto> dto)
    {
        _operationGraphDetails = dto
            .Select(d => new OperationGraphDetail()
            {
                DetailId = d.DetailId,
                Usability = d.Usability,
                DetailGraphNumberWithRepeats = d.PositionNumber
            })
            .ToList();
    }

    public OperationGraphDetailBuilder(List<OperationGraphDetail> details)
    {
        _operationGraphDetails = details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="planCount"></param>
    public void CalculatePlannedNumber(float planCount)
    {
        var detailRepository = new OperationGraphDetailRepository();

        foreach (var detail in _operationGraphDetails)
        {
            var usabilityWithFathers = detailRepository.CalculateUsabilityWithFathers(_operationGraphDetails, detail);

            detail.UsabilityWithFathers = usabilityWithFathers;
            detail.PlannedNumber = usabilityWithFathers * planCount;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateTotalPlannedNumber()
    {
        var ingoredDetails = new List<int>();

        foreach (var detail in _operationGraphDetails.Where(detail => !ingoredDetails.Contains(detail.DetailId)))
        {
            detail.TotalPlannedNumber = _operationGraphDetails
                .Where(d => d.DetailId == detail.DetailId)
                .Select(d => d.PlannedNumber)
                .Sum();

            ingoredDetails.Add(detail.DetailId);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateUsabilitySum()
    {
        foreach (var detail in _operationGraphDetails.Where(detail => detail.TotalPlannedNumber.HasValue))
        {
            detail.UsabilitySum = 0;
            foreach (var repeatedDetail in _operationGraphDetails.Where(d => d.DetailId == detail.DetailId))
            {
                detail.UsabilitySum += repeatedDetail.UsabilityWithFathers;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void GroupRepeatingDetails()
    {
        foreach (var detail in _operationGraphDetails.Where(d => d.TotalPlannedNumber.HasValue))
        {
            var slaveDetails = _operationGraphDetails.Where(d => d.DetailId == detail.DetailId && !d.TotalPlannedNumber.HasValue);
            detail.OperationGraphMainDetails = new List<OperationGraphDetailGroup>();
            detail.OperationGraphMainDetails.AddRange(slaveDetails.Select(d => new OperationGraphDetailGroup
            {
                OperationGraphNextDetail = d
            }));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateNumberWithoutRepeats()
    {
        var i = 0;
        foreach (var detail in _operationGraphDetails.Where(detail => detail.TotalPlannedNumber.HasValue))
        {
            detail.DetailGraphNumberWithoutRepeats = ++i;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task SetTechProcessIds(DbContext context)
    {
        var techProcesses = await context.Set<TechnologicalProcess>()
            .Where(tp =>
                DetailsIdWithoutRepeating.Contains(tp.DetailId)
                && tp.ManufacturingPriority == 1
                && tp.DevelopmentPriority == 0)
            .Select(tp => new { Key = tp.DetailId, Value = tp.Id })
            .ToDictionaryAsync(k => k.Key, v => v.Value);

        foreach (var techProcess in techProcesses)
            _operationGraphDetails.Single(d => d.TotalPlannedNumber.HasValue && d.DetailId == techProcess.Key).TechnologicalProcessId = techProcess.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateNumber()
    {
        var i = 0;
        foreach (var detail in _operationGraphDetails)
        {
            detail.DetailGraphNumber = ++i;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateNumber(int start)
    {
        var i = start;
        foreach (var detail in _operationGraphDetails)
        {
            detail.DetailGraphNumber = ++i;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    public void SetItems(List<OperationGraphDetailItemAddingInfo> items)
    {
        foreach (var detail in _operationGraphDetails)
        {
            detail.OperationGraphDetailItems = new List<OperationGraphDetailItem>();
            detail.OperationGraphDetailItems.AddRange(items
                .Where(di => di.DetailId == detail.DetailId && detail.TotalPlannedNumber.HasValue)
                .Select(di => new OperationGraphDetailItem
                {
                    OrdinalNumber = di.OrdinalNumber,
                    TechnologicalProcessItemId = di.TechnologicalProcessItemId
                })
            );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override List<OperationGraphDetail> Build() => _operationGraphDetails;
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<List<OperationGraphDetail>> BuildAsync() => _operationGraphDetails;
}