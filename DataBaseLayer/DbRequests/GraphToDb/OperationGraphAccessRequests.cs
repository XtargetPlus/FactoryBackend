using AutoMapper;
using AutoMapper.QueryableExtensions;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Graph.Access;

namespace DatabaseLayer.DbRequests.GraphToDb;

public class OperationGraphAccessRequests
{
    private readonly DbSet<OperationGraphUser> _graphUser;
    private readonly IMapper _dataMapper;

    public OperationGraphAccessRequests(DbContext context, IMapper dataMapper)
    {
        _graphUser = context.Set<OperationGraphUser>();
        _dataMapper = dataMapper;
    }

    public async Task<List<GetAllUserGraphAccessDto>?> GetAllUsersWithAccessAsync(int graphId)
    {
        try
        {
            return await _graphUser
                .Where(gu => gu.OperationGraphId == graphId)
                .ProjectTo<GetAllUserGraphAccessDto>(_dataMapper.ConfigurationProvider)
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