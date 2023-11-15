using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Tools;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Enums;

namespace ServiceLayer.Tools.Services;

public class ToolsService : ErrorsMapper, IToolsService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Tool> _repository;
    private readonly IMapper _dataMapper;

    public ToolsService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<Tool>(context, dataMapper);
    }

    public async Task<int?> AddAsync(AddToolDto dto)
    {
        var tool = _dataMapper.Map<Tool>(dto);

        tool.ToolCatalogsConcretes = new List<ToolCatalogConcrete>
        {
            new()
            {
                ToolCatalogId = dto.CatalogId
            }
        };

        await _context.AddWithValidationsAndSaveAsync(tool, this);
        return tool.Id;
    }

    public async Task<List<GetToolDto>?> GetForCatalogAsync(int? catalogId) =>
         await _repository.GetAllAsync<GetToolDto>(filter: t =>
             t.ToolCatalogsConcretes!.Any(cc => catalogId.HasValue && cc.ToolCatalogId == catalogId));

    public async Task<List<GetToolDto>?> GetWithParameters(GetToolWithParametersDto dto)
    {
        var parameterConcretes = await new BaseModelRequests<ToolParameterConcrete>(_context, _dataMapper).GetAllAsync();
        
        foreach (var item in dto.ParametersFilters!)
        {
            switch (item.ConditionalStatement)
            {
                case ToolParameterConditionalStatements.Equal:
                    parameterConcretes.RemoveAll(t => !(t.Value == item.Value && t.ToolParameterId == item.ParameterId));
                    break;
                case ToolParameterConditionalStatements.NotEqual:
                    parameterConcretes.RemoveAll(t => !(t.Value != item.Value && t.ToolParameterId == item.ParameterId));
                    break;
                case ToolParameterConditionalStatements.Less:
                    parameterConcretes.RemoveAll(t => !(Convert.ToInt32(t.Value) < Convert.ToInt32(item.Value) && t.ToolParameterId == item.ParameterId));
                    break;
                case ToolParameterConditionalStatements.Greater:
                    parameterConcretes.RemoveAll(t => !(Convert.ToInt32(t.Value) > Convert.ToInt32(item.Value) && t.ToolParameterId == item.ParameterId));
                    break;
                case ToolParameterConditionalStatements.LessOrEqual:
                    parameterConcretes.RemoveAll(t => !(Convert.ToInt32(t.Value) <= Convert.ToInt32(item.Value) && t.ToolParameterId == item.ParameterId));
                    break;
                case ToolParameterConditionalStatements.GreaterOrEqual:
                    parameterConcretes.RemoveAll(t => !(Convert.ToInt32(t.Value) >= Convert.ToInt32(item.Value) && t.ToolParameterId == item.ParameterId));
                    break;
            }
        }

        var getTool = new List<GetToolDto>();
        foreach (var item in parameterConcretes)
        {
            getTool.AddRange(await _repository.GetAllAsync<GetToolDto>(filter: t => t.Id == item.ToolId));
        }

        return getTool;
    }
    public async Task ChangeAsync(ChangeToolDto dto)
    {
        var tool = await _repository.FindFirstAsync(
            filter: tc => (tc.Id == dto.Id),
            include: i => i.Include(tc => tc.ToolCatalogsConcretes));
        if (tool is null)
        {
            AddErrors("Не удалось найти инструмент");
            return;
        }

        if (tool.Title != dto.Title)
            tool.Title = dto.Title;
        if (tool.SerialNumber != dto.Serial)
            tool.SerialNumber = dto.Serial;
        if (tool.Note != dto.Note)
            tool.Note = dto.Note;

        tool.ToolCatalogsConcretes = new List<ToolCatalogConcrete>
        {
            new()
            {
                ToolCatalogId = dto.CatalogId
            }
        };

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteAsync(int id)
    {
        _context.Remove(new Tool { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    public void Dispose() => _context.Dispose();


}