using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;
using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.DbRequests.ToolToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.ToolInfo;
using Shared.Dto.Tools.ToolChild.Filters;
using Shared.Enums;

namespace ServiceLayer.Tools.Services;

public class ToolChildService : ErrorsMapper, IToolChildService
{
    private readonly DbApplicationContext _context;
    private readonly IMapper _dataMapper;
    private readonly BaseModelRequests<ToolChild> _repository;
    public ToolChildService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<ToolChild>(context, dataMapper);
    }

    public async Task AddChildAsync(AddToolChildrenDto dto)
    {
        if (dto.FatherId == dto.ChildrenId)
        {
            AddErrors("Добавление инструмента самого в себя");
            return;
        }

        await _context.AddWithValidationsAndSaveAsync(new ToolChild
        {
            FatherId = dto.FatherId,
            ChildId = dto.ChildrenId,
            Count = dto.Count,
            Priority = dto.Priority
        }, this);
    }

    public async Task ChangeChildAsync(ChangeToolChildDto dto)
    {
        var toolChild = await _repository.FindFirstAsync(filter: tc => (tc.FatherId == dto.FatherId && tc.ChildId == dto.ChildId));
        if (HasErrors) return;

        if (dto.Count != toolChild!.Count)
        {
            toolChild.Count = dto.Count;   
        }
                
        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task InsertBetweenAsync(SwapToolChildDto dto)
    {
        var min = Math.Min(dto.NewPrioryty, dto.OldPrioryty);
        var max = Math.Max(dto.NewPrioryty, dto.OldPrioryty);

        var decInc = 1;
        if (dto.NewPrioryty > dto.OldPrioryty)
            decInc = -1;

        var toolChild = await _repository.GetAllAsync(
            filter: tc => tc.FatherId == dto.FatherId && tc.Priority >= min && tc.Priority <= max,
            trackingOptions: TrackingOptions.WithTracking);
        
        if (toolChild.Count == 0)
        {
            AddErrors("У инструмента нет дочерних инструментов");
            return;
        }
        if(toolChild.Count == 1)
        {
            AddErrors("У инструмента только один дочерний инструмент");
            return;
        }

        toolChild.ForEach(tc => tc.Priority += decInc);
        toolChild.Find(tc => tc.Priority == (dto.OldPrioryty + decInc)).Priority = dto.NewPrioryty;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteChildAsync(DeleteToolChildDto dto)
    {
        var toolChild = await _repository.FindFirstAsync(filter: tc => (tc.FatherId == dto.FatherId && tc.ChildId == dto.ChildId));
        if (HasErrors) return;

        await _context.RemoveWithValidationAndSaveAsync(toolChild, this);
    }

    public async Task SwapChildAsync(SwapToolChildDto dto)
    {
        var toolChilds = await _repository.GetAllAsync(filter: tc => tc.FatherId == dto.FatherId && (tc.Priority == dto.NewPrioryty || tc.Priority == dto.OldPrioryty),
            trackingOptions: TrackingOptions.WithTracking);
        
        if(toolChilds is null)
        {
            AddErrors("Неверно указан родительский инструмент");
            return;
        }

        if (toolChilds.Count == 0)
        {
            AddErrors("У инструмента нет дочерних инструментов");
            return;
        }

        toolChilds.Find(t => t.Priority == dto.OldPrioryty)!.Priority = dto.NewPrioryty;
        toolChilds.Find(t => t.Priority == dto.NewPrioryty)!.Priority = dto.OldPrioryty;
        
        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task<List<GetToolChildDto>?> GetAllAsync(GetAllChildrenFilters filters) =>
        await new ToolChildRequests(_context, _dataMapper).GetAllChildrenAsync(filters);

    public async Task<List<GetToolFatherDto>?> GetAllFatherAsync(GetAllFatherFilters filters) =>
        await new ToolChildRequests(_context, _dataMapper).GetAllFatherAsync(filters);

    public void Dispose() => _context.Dispose();
}