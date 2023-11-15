using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.ToolO;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Tools;
using Shared.Dto.Tools.ToolParameters.Filters;

namespace DatabaseLayer.DbRequests.ToolToDb;

public class ToolParametersRequests
{
    private readonly DbSet<ToolParameterConcrete> _toolParameter;
    private readonly IMapper _dataMapper;

    public ToolParametersRequests(DbContext context, IMapper mapper)
    {
        _toolParameter = context.Set<ToolParameterConcrete>();
        _dataMapper = mapper;
    }

    public async Task<List<GetToolParametersDto>?> GetAllParameterAsync(GetAllParametersFilters filters)
    {
        try
        {
            return await _toolParameter
                .AsNoTracking()
                .ToolParameterOrder(filters.KindOfOrder)
                .Where(tpc => tpc.ToolId == filters.ToolId)
                .ProjectTo<GetToolParametersDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ничего не найдено: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
        }

        return null;
    }
}