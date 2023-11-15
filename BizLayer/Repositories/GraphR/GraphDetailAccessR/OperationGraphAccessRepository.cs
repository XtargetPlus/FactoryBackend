using DB.Model.StorageInfo.Graph;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR.GraphDetailAccessR;

public class OperationGraphAccessRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="usersId"></param>
    public void RemoveRange(OperationGraph graph, List<int> usersId)
    {
        usersId.ForEach(id => graph.OperationGraphUsers!.Remove(graph.OperationGraphUsers.First(gu => gu.UserId == id)));
        usersId.ForEach(id => graph.OperationGraphMainGroups!.ForEach(gg =>
            gg.OperationGraphNext!.OperationGraphUsers!.Remove(gg.OperationGraphNext.OperationGraphUsers.First(gu => gu.UserId == id))
        ));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="userId"></param>
    /// <param name="newAccess"></param>
    public void ChangeRange(OperationGraph graph, int userId, NewUserAccess newAccess)
    {
        graph.OperationGraphUsers!.Single(gu => gu.UserId == userId).IsReadonly = newAccess == NewUserAccess.Readonly;
        graph.OperationGraphMainGroups!.ForEach(gg =>
            gg.OperationGraphNext!.OperationGraphUsers!.Single(gu => gu.UserId == userId).IsReadonly = newAccess == NewUserAccess.Readonly
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="ownerId"></param>
    public void SetNewOwner(OperationGraph graph, int ownerId)
    {
        graph.OwnerId = ownerId;
        graph.OperationGraphMainGroups!.ForEach(gg => gg.OperationGraphNext!.OwnerId = ownerId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="usersAccess"></param>
    public void AddRange(OperationGraph graph, Dictionary<int, bool> usersAccess)
    {
        graph.OperationGraphUsers!.AddRange(usersAccess.Select(ua => new OperationGraphUser
        {
            IsReadonly = ua.Value,
            UserId = ua.Key
        }));
        graph.OperationGraphMainGroups!.ForEach(gg => gg.OperationGraphNext!.OperationGraphUsers!.AddRange(
            usersAccess.Select(ua => new OperationGraphUser
            {
                IsReadonly = ua.Value,
                UserId = ua.Key
            }))
        );
    }
}