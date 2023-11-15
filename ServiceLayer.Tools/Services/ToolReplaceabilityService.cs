using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.DbRequests.ToolToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.ToolInfo;
using DB;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolReplaceability.Filters;

namespace ServiceLayer.Tools.Services;

public class ToolReplaceabilityService : ErrorsMapper, IToolReplaceabilityService
{

    private readonly DbApplicationContext _context;
    private readonly IMapper _dataMapper;
    private readonly BaseModelRequests<ToolReplaceability> _repository;

    public ToolReplaceabilityService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<ToolReplaceability>(context, dataMapper);
    }

    public async Task AddReplaceabilityAsync(AddReplaceabilityDto dto)
    {
        if (dto.MasterId == dto.SlaveId)
        {
            AddErrors("Добавление заменяемости самой в себя");
            return;
        }

        await _context.AddWithValidationsAndSaveAsync(new ToolReplaceability
        {
            MasterId = dto.MasterId,
            SlaveId = dto.SlaveId,
        }, this);
    }

    public async Task ChangeReplaceabilityAsync(AddReplaceabilityDto dto)
    {
        var toolReplaceability = await _repository.FindFirstAsync(filter: tr => (tr.MasterId == dto.MasterId && tr.SlaveId == dto.SlaveId));
        if (HasErrors) return;

        if (dto.SlaveId != toolReplaceability!.SlaveId)
        {
            toolReplaceability.SlaveId = dto.SlaveId;
            await _context.SaveChangesWithValidationsAsync(this);
        }
    }

    public async Task DeleteReplaceabilityAsync(AddReplaceabilityDto dto)
    {
        var toolReplaceability = await _repository.FindFirstAsync(filter: tr => (tr.MasterId == dto.MasterId && tr.SlaveId == dto.SlaveId));
        if (HasErrors) return;

        await _context.RemoveWithValidationAndSaveAsync(toolReplaceability, this);
    }

    public async Task<List<GetToolReplaceabilityDto>?> GetReplaceabilityAsync(GetAllReplaceabilityFilters filters)
    {
        var allReplaceability = new List<int> { filters.ToolId };
        var allReplaceabilityDto = new List<GetToolReplaceabilityDto>();
        
        for (var i = 0; i < allReplaceability.Count; i++)
        {
            filters.ToolId = allReplaceability[i];
            var tempList = await new ToolReplaceabilityRequests(_context, _dataMapper).GetAllReplaceabilityAsync(filters);

            allReplaceabilityDto.AddRange(tempList);

            allReplaceability.AddRange(tempList.Select(aa => aa.Id).Distinct());
        }
        
        return allReplaceabilityDto;
    }

    public void Dispose() => _context.Dispose();
}