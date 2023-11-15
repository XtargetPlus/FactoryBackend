using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLayer.Options;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Read;
using Shared.Dto.Users;
using Shared.Dto.Users.Filters;

namespace DatabaseLayer.IDbRequests.UserToDb;

public class UserRequests
{
    private readonly DbSet<User> _user; 
    private readonly IMapper _dataMapper;

    public UserRequests(DbContext context, IMapper dataMapper)
    {
        _user = context.Set<User>();
        _dataMapper = dataMapper;
    }

    public async Task<List<UserGetWithSubdivisionDto>?> GetProfAll(GetAllUserFromProfessionFilters filters)
    {
        try
        {
            return await _user
                .Where(u => u.ProfessionId == filters.ProfessionId)
                .UserProfessionOrder(filters.OrderOptions, filters.KindOfOrder)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<UserGetWithSubdivisionDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<UserGetProfessionDto>?> GetSubdivAll(GetAllUserFromSubdivisionFilters filters)
    {
        try
        {
            return await _user
                .Where(u => u.SubdivisionId == filters.SubdivisionId)
                .UserSubdivOrder(filters.OrderOptions, filters.KindOfOrder)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<UserGetProfessionDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<UserGetDto>?> GetAll(GetAllUserFilters filters)
    {
        try
        {
            return await _user
                .UserSearch(filters.SearchOptions, filters.Text)
                .UserOrder(filters.OrderOptions, filters.KindOfOrder)
                .UserSelectFromStatus(filters.StatusId)
                .Skip(filters.Skip)
                .Take(filters.Take)
                .AsNoTracking()
                .ProjectTo<UserGetDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<UserGetWithSubdivisionDto>?> GetAllWithoutAccessToOperationGraphAsync(int graphId)
    {
        try
        {
            return await _user
                .Where(u => !u.OperationGraphUsers.Any(gu => gu.OperationGraphId == graphId) && !u.OperationGraphs.Any(g => g.Id == graphId))
                .AsNoTracking()
                .ProjectTo<UserGetWithSubdivisionDto>(_dataMapper.ConfigurationProvider)
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

    public async Task<List<GetAllDevelopersTasksDto>?> GetAllTechnologists(List<int> developerIds)
    {
        try
        {
            return await _user
                .AsNoTracking()
                .Where(u => developerIds.Contains(u.Id))
                .ProjectTo<GetAllDevelopersTasksDto>(_dataMapper.ConfigurationProvider)
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