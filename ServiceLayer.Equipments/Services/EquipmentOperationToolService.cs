using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Equipments.Services.Interfaces;
using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;

namespace ServiceLayer.Equipments.Services;

public class EquipmentOperationToolService : ErrorsMapper, IEquipmentOperationToolService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<EquipmentOperationTool> _repository;
    private readonly IMapper _dataMapper;
    public EquipmentOperationToolService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<EquipmentOperationTool>(context, dataMapper);
    }
    public async Task AddEquipmentOperationTool(AddEquipmentOperationTool dto)
    {
        var equipmentOperation = new BaseModelRequests<EquipmentOperation>(_context, _dataMapper);

        var t = await _context.AddWithValidationsAndSaveAsync(new EquipmentOperationTool
        {
            EquipmentOperationId = dto.EquipmentOperationId,
            Priority = dto.Priority,
            Count = dto.Count,
            ToolId = dto.Tools[0],
            FatherId = 0
        },this);
        var list = new List<EquipmentOperationTool>();
        for(int i = 1; i < dto.Tools.Count; i++)
        {
            list.Add(new()
            {
                EquipmentOperationId = dto.EquipmentOperationId,
                Priority = dto.Priority,
                Count = dto.Count,
                ToolId = dto.Tools[i],
                FatherId = t.Id
            });
        }
        await _context.AddWithValidationsAndSaveAsync(list,this);
    }

    public async Task<GetAllEquipmentOperationToolsDto> GetEquipmentOperationTool(GetAllEquipmentOperationToolsFilters dto)
    {
        
        var equipmentOperationTools = await _repository.GetAllAsync(filter: eot => eot.EquipmentOperationId == dto.EquipmentOperationId && eot.FatherId == 0, include: i => i.Include(eot => eot.Tool));

        GetAllEquipmentOperationToolsDto equipmnetTools = new();
        equipmnetTools.EquipmentOperationId = dto.EquipmentOperationId;

        foreach(var t in equipmentOperationTools)
        {
            var r = await _repository.GetAllAsync(filter: eot => eot.EquipmentOperationId == dto.EquipmentOperationId && eot.FatherId == t.Id, include: i => i.Include(eot => eot.Tool));
            List<Tool> v = new() { t.Tool};
            foreach(var i in r)
            {
                v.Add(i.Tool);
            }
            equipmnetTools.Tools.Add(v);
        }

        return equipmnetTools;
    }

    public void Dispose() => _context.Dispose();

    
}
