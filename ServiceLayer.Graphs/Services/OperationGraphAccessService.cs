using DatabaseLayer.Helper;
using ServiceLayer.Graphs.Services.Interfaces;
using AutoMapper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using DB;
using BizLayer.Repositories.GraphR;
using BizLayer.Repositories.GraphR.GraphDetailAccessR;
using DatabaseLayer.DatabaseChange;
using Shared.Enums;
using Shared.Dto.Graph.Access;
using DatabaseLayer.DbRequests.GraphToDb;
using DatabaseLayer.IDbRequests.UserToDb;
using Shared.Dto.Users;

namespace ServiceLayer.Graphs.Services;

public class OperationGraphAccessService : ErrorsMapper, IOperationGraphAccessService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraph> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphAccessService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Смена владельца графика с возможностью оставления у старого владельца прав доступа на чтение или редактирование
    /// </summary>
    /// <param name="dto">Информация для смены владельца</param>
    /// <returns></returns>
    public async Task ChangeOwnerAsync(ChangeGraphOwnerDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.OperationGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphRead.LoadUsersWithAccessAsync(_repository, graph, this);
        
        if (HasErrors) return;

        var graphAccessRepository = new OperationGraphAccessRepository();

        var oldOwnerId = graph!.OwnerId;

        // если новый владелец есть в списке пользователей с доступом к этому графику - удаляем его из списка
        if (graph.OperationGraphUsers!.Any(gu => gu.UserId == dto.NewOwnerId))
            graphAccessRepository.RemoveRange(graph, new List<int>(dto.NewOwnerId));

        graphAccessRepository.SetNewOwner(graph, dto.NewOwnerId);

        // если старый пользователь решил оставить за собой права
        if (dto.NewUserAccess != NewUserAccess.None)
        {
            graphAccessRepository.AddRange(graph, new Dictionary<int, bool>()
            {
                { oldOwnerId, dto.NewUserAccess != NewUserAccess.ReadAndEdit }
            });
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Предоставление доступа к графику или группе графиков, другим пользователям
    /// </summary>
    /// <param name="dto">Информация для предоставления доступа</param>
    /// <returns></returns>
    public async Task GiveAccessAsync(GiveAccessGraphDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.OperationGraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        
        if (HasErrors) return;

        graph!.OperationGraphUsers = new List<OperationGraphUser>();
        graph.OperationGraphMainGroups!.ForEach(gg => gg.OperationGraphNext!.OperationGraphUsers = new List<OperationGraphUser>());

        var graphAccessRepository = new OperationGraphAccessRepository();

        graphAccessRepository.AddRange(graph, dto.UserAccesses);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Меняем пользователю права доступа к графику
    /// </summary>
    /// <param name="dto">Информация для смены прав доступа</param>
    /// <returns></returns>
    public async Task ChangeUserAccess(ChangeUserAccessDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphRead.LoadUsersWithAccessAsync(_repository, graph, this);
        if (!graph?.OperationGraphUsers?.Any(gu => gu.UserId == dto.UserId) ?? true)
            AddErrors("В списке пользователей с доступом нет переданного пользователя");
        
        if (HasErrors) return;

        var graphAccessRepository = new OperationGraphAccessRepository();

        graphAccessRepository.ChangeRange(graph!, dto.UserId, dto.NewAccess);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Отбираем у пользователя доступ к графику
    /// </summary>
    /// <param name="dto">Информация для изъятия доступа</param>
    /// <returns></returns>
    public async Task RevokeAccess(RevokeGraphAccessDto dto)
    {
        var graph = await OperationGraphRead.ByIdAsync(_context, dto.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, graph, this);
        await OperationGraphRead.LoadUsersWithAccessAsync(_repository, graph, this);
        if (!graph?.OperationGraphUsers?.Any(gu => dto.UsersId.Contains(gu.UserId)) ?? true)
            AddErrors("В списке пользователей с доступом нет переданного пользователя");

        if (HasErrors) return;

        var graphAccessRepository = new OperationGraphAccessRepository();

        graphAccessRepository.RemoveRange(graph!, dto.UsersId);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Получение списка пользователей, имеющих доступ к переденному графику
    /// </summary>
    /// <param name="graphId">Id операционного графика</param>
    /// <returns>Список пользователей, которые имеют доступ к графику</returns>
    public async Task<List<GetAllUserGraphAccessDto>?> AllWithAccessToOperationGraphAsync(int graphId) =>
        await new OperationGraphAccessRequests(_context, _dataMapper).GetAllUsersWithAccessAsync(graphId);

    /// <summary>
    /// Получаем список пользователей, которые не имеют доступ к графику
    /// </summary>
    /// <param name="graphId">Id графика</param>
    /// <returns>Список пользователей без доступа к графику</returns>
    public async Task<List<UserGetWithSubdivisionDto>?> AllWithoutAccessToOperationGraphAsync(int graphId) =>
        await new UserRequests(_context, _dataMapper).GetAllWithoutAccessToOperationGraphAsync(graphId);

    /// <summary>
    /// Получаем список видов доступа к графику
    /// </summary>
    /// <returns>Список видов доступа к графику</returns>
    public Dictionary<int, string> AccessTypes() =>
        new()
        {
            { 1, "Только на чтение" },
            { 2, "Полный" }
        };

    public void Dispose() => _context.Dispose();
}