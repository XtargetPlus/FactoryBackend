using AutoMapper;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.ToolInfo;
using DB;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.Dto.Tools;

namespace ServiceLayer.Tools.Services;

public class ToolParameterService : ErrorsMapper, IToolParameterService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<ToolParameter> _repository;
    public ToolParameterService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new BaseModelRequests<ToolParameter>(context, dataMapper);
    }
    public async Task<int?> AddParameterAsync(AddToolParameterDto dto)
    {
        var toolParameter = await _context.AddWithValidationsAndSaveAsync(
            new ToolParameter()
            {
                Title = dto.Title,
                UnitId = dto.UnitId
            }, this);
        return toolParameter?.Id;
    }

    public async Task ChangeParameterAsync(ChangeToolParameterDto dto)
    {
        var toolParameter = await _repository.FindByIdAsync(dto.Id);

        if (toolParameter is null)
        {
            AddErrors("Не удалось найти параметр");
            return;
        }

        if (toolParameter.Title != dto.Title && dto.Title is not null)
            toolParameter.Title = dto.Title;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteParameterAsync(int id)
    {
        _context.Remove(new ToolParameter() { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    public async Task<List<GetToolParameterDto>?> GetParametersAsync() => await _repository.GetAllAsync<GetToolParameterDto>();

    public void Dispose() => _context.Dispose();
}