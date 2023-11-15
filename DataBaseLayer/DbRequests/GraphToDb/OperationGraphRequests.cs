using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options.GraphO;
using DB.Model.StorageInfo.Graph;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Filters;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace DatabaseLayer.DbRequests.GraphToDb;

public class OperationGraphRequests
{
    private readonly DbSet<OperationGraph> _operationGraph;
    private readonly IMapper _dataMapper;

    public OperationGraphRequests(DbContext context, IMapper dataMapper)
    {
        _operationGraph = context.Set<OperationGraph>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GetAllOperationGraphDto>?> GetAllAsync(GetAllOperationGraphFilters filters, int userId)
    {
        try
        {
            return await _operationGraph
                .AsNoTracking()
                .FromDate(filters.StartDate, filters.EndDate)
                .FromSubdivisionId(filters.SubdivisionId)
                .FromStatusId((int)filters.Status)
                .FromOwnershipType(filters.OwnershipType, userId)
                .FromAccess(userId, filters.AccessTypeForFilters)
                .FromProductAavailability(filters.ProductAvailability)
                .ProjectTo<GetAllOperationGraphDto>(_dataMapper.ConfigurationProvider, new { userId })
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

    public async Task<List<GetAllOperationGraphDto>?> GroupAsync(int priority, int userId)
    {
        try
        {
            return await _operationGraph
                .AsNoTracking()
                .Where(g => g.Priority == priority)
                .ProjectTo<GetAllOperationGraphDto>(_dataMapper.ConfigurationProvider, new { userId })
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

    public async Task<List<GetAllSinglesOperationGraphDto>?> GetAllSinglesForGroupAsync(GetAllSinglesForGroupFilters filters, int userId)
    {
        try
        {
            return await _operationGraph
                .AsNoTracking()
                .Where(g => (g.StatusId == (int)GraphStatus.InWork || g.StatusId == (int)GraphStatus.Paused) 
                            && g.OperationGraphMainGroups!.Count == 0 
                            && g.OperationGraphNextGroups!.Count == 0
                            && g.OperationGraphUsers!.Count == 0)
                .FromIgnoredGraphIds(filters.IgnoredGraphIds)
                .FromProductAavailability(filters.ProductAvailability)
                .FromOwnershipType(GraphOwnershipType.Owner, userId)
                .ProjectTo<GetAllSinglesOperationGraphDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<GetAllOperationGraphFromOwnerDto>?> GetAllFromOwnerAsync(GetAllOperationGraphFromOwnerFilters filters, int userId)
    {
        try
        {
            return await _operationGraph
                .AsNoTracking()
                .Where(g => g.StatusId == (int)GraphStatus.InWork || g.StatusId == (int)GraphStatus.Paused)
                .FromOwnershipType(GraphOwnershipType.Owner, userId)
                .FromProductAavailability(filters.ProductAvailability)
                .ProjectTo<GetAllOperationGraphFromOwnerDto>(_dataMapper.ConfigurationProvider, new { userId })
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

    public async Task<GraphInfoDto?> OpenAsync(int graphId, int userId)
    {
        try
        {
            return await _operationGraph
                .Where(g => g.Id == graphId)
                .ProjectTo<GraphInfoDto>(_dataMapper.ConfigurationProvider, new { userId })
                .SingleAsync();
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