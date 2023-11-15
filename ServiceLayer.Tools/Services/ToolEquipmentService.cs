using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.DbRequests.ToolToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.ToolInfo;
using DB;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolEquipment.Filters;

namespace ServiceLayer.Tools.Services;

public class ToolEquipmentService : ErrorsMapper, IToolEquipmentService
{
    private readonly DbApplicationContext _context;
    private readonly IMapper _dataMapper;
    private readonly BaseModelRequests<EquipmentTool> _repository;

    public ToolEquipmentService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<EquipmentTool>(context, _dataMapper);
    }
    public async Task AddEquipmentAsync(AddToolEquipmentDto dto)
    {
        var toolEquipment = await _repository.FindFirstAsync(filter: et =>
                (et.EquipmentId == dto.EquipmentId && et.ToolId == dto.ToolId));

        if (toolEquipment != null)
        {
            AddErrors("Связь существует");
            return;
        }

        await _context.AddWithValidationsAndSaveAsync(
            new EquipmentTool
            {
                ToolId = dto.ToolId,
                EquipmentId = dto.EquipmentId,
            }, this);
    }

    public async Task ChangeEquipmentAsync(AddToolEquipmentDto dto)
    {
        var equipmentTool = await _repository.FindFirstAsync(filter: et =>
                (et.EquipmentId == dto.EquipmentId && et.ToolId == dto.ToolId));
        if (HasErrors) return;

        if (equipmentTool?.EquipmentId != dto.EquipmentId)
        {
            equipmentTool.EquipmentId = dto.EquipmentId;
            await _context.SaveChangesWithValidationsAsync(this);
        }
    }

    public async Task DeleteEquipmentAsync(AddToolEquipmentDto dto)
    {
        var equipmentTool = await _repository.FindFirstAsync(filter: et =>
                (et.EquipmentId == dto.EquipmentId && et.ToolId == dto.ToolId));
        if (HasErrors) return;

        await _context.RemoveWithValidationAndSaveAsync(equipmentTool, this);
    }

    public async Task<List<GetToolEquipmentDto>?> GetEquipmentAsync(GetAllEquipmentFilters filters) =>
        await new ToolEquipmentRequests(_context, _dataMapper).GetAllEquipmentAsync(filters);

    public void Dispose() => _context.Dispose();
}