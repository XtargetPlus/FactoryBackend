using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.DbRequests.ToolToDb;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.ToolInfo;
using DB;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;
using Shared.Dto.Detail.DetailChild.Filters;
using Shared.Dto.Tools.ToolParameters.Filters;

namespace ServiceLayer.Tools.Services;

public class ToolParameterConcreteService : ErrorsMapper, IToolParameterConcreteService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Tool> _repository;
    private readonly IMapper _dataMapper;

    public ToolParameterConcreteService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<Tool>(context, dataMapper);
    }

    public async Task AddRangeParametersAsync(AddToolParametersListDto dto)
    {
        var tool = await _repository.FindByIdAsync(dto.ToolId);
        if (tool is null)
        {
            AddErrors("Не удалось найти инструмент");
            return;
        }

        tool.ParametersConcrete = new List<ToolParameterConcrete>();

        tool.ParametersConcrete.AddRange(dto.AddToolParameters.Select(tp => new ToolParameterConcrete
        {
            Tool = tool,
            ToolParameterId = tp.ParameterId,
            Value = tp.Value
        }));

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task ChangeRangeParametersAsync(AddToolParametersListDto dto)
    {
        var tool = await _repository.FindFirstAsync(filter: t => t.Id == dto.ToolId, include: i => i.Include(t => t.ParametersConcrete));
        if (tool is null)
        {
            AddErrors("Не удалось найти инструмент");
            return;
        }

        tool.ParametersConcrete!.ForEach(pc =>
            pc.Value = dto.AddToolParameters.Single(p => p.ParameterId == pc.ToolParameterId).Value);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteRangeParametersAsync(DeleteToolParametersDto dto)
    {
        var tool = await _repository.FindFirstAsync(
            filter: t => t.Id == dto.ToolId,
            include: i => i.Include(t => t.ParametersConcrete));
        if (tool is null)
        {
            AddErrors("Не удалось найти инструмент");
            return;
        }

        dto.ParametersId!.ForEach(id => tool.ParametersConcrete!.Remove(new ToolParameterConcrete { ToolParameterId = id }));

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task<List<GetToolParametersDto>?> GetAllRangeParametersAsync(GetAllParametersFilters filters) =>
        await new ToolParametersRequests(_context, _dataMapper).GetAllParameterAsync(filters);

    public void Dispose() => _context.Dispose();


}