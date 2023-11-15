using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.TechologicalProcessO;
using DB.Model.StatusInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.Filters;
using Shared.Dto.TechnologicalProcess.Issue.Filters;
using Shared.Dto.TechnologicalProcess.Issue.Read;
using Shared.Static;

namespace DatabaseLayer.DbRequests.TechnologicalProcessToDb;

public class TpStatusesRequest
{
    private readonly DbSet<TechnologicalProcessStatus> _techProcessStatus;
    private readonly IMapper _dataMapper;

    public TpStatusesRequest(DbContext context, IMapper dataMapper)
    {
        _techProcessStatus = context.Set<TechnologicalProcessStatus>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GeTechProcessDevelopmentStagesDto>?> GetDevelopmentStagesAsync(GetDevelopmentStagesFilters filters)
    {
        try
        {
            return await _techProcessStatus
                .AsNoTracking()
                .TpDevelopmentStagesOrder(filters.OrderOptions, filters.KindOfOrder)
                .Where(tps => tps.TechnologicalProcessId == filters.TechProcessId)
                .ProjectTo<GeTechProcessDevelopmentStagesDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync(); ;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ничего не найдено: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
        }
        return null;
    }

    public async Task<List<GetBaseTechProcessDto>?> GetAllCompletedDeveloperTpsAsync(GetAllCompletedDeveloperTechProcessesFilters filters)
    {
        try
        {
            var result = await _techProcessStatus
                .IgnoreQueryFilters()
                .AsNoTracking()
                .CompletedTpsOrder(filters.OrderOptions, filters.KindOfOrder)
                .Where(tps => tps.TechnologicalProcess!.DeveloperId == filters.DeveloperId && tps.StatusId == (int)TechProcessStatuses.Completed)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .ProjectTo<GetBaseTechProcessDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();
            
            return result.Distinct().ToList();
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ничего не найдено: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
        }
        return null;
    }

    public async Task<List<GetTechProcessDuplicateDto>?> GetAllIssuedDuplicatesTechProcessAsync(GetAllIssuedDuplicatesTechProcessFilters filters)
    {
        try
        {
            return await _techProcessStatus
                .IssuedDuplicatesOrder(filters.OrderOptions, filters.KindOfOrder)
                .Where(tps => tps.TechnologicalProcessId == filters.TechProcessId && tps.StatusId == (int)TechProcessStatuses.IssuedDuplicate)
                .ProjectTo<GetTechProcessDuplicateDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ничего не найдено: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
        }
        return null;
    }

    public async Task<List<IssuedTechProcessesFromTechnologistDto>?> GetAllIssuedTechProcessesFromTechnologistAsync(IssuedTechProcessesFromTechnologistRequestFilters requestOptions, int userId)
    {
        try
        {
            return await _techProcessStatus
                .Where(tps => tps.TechnologicalProcess!.DeveloperId == userId && tps.StatusId == (int)TechProcessStatuses.IssuedDuplicate)
                .IssuedTechProcessesFromTechnologistOrder(requestOptions.OrderOptions, requestOptions.KindOfOrder)
                .Skip(requestOptions.Skip)
                .Take(requestOptions.Take)
                .ProjectTo<IssuedTechProcessesFromTechnologistDto>(_dataMapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (ArgumentNullException ex)
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