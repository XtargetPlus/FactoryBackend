using System.ComponentModel.DataAnnotations;

namespace DB.Model.UserInfo.RoleInfo;

/// <summary>
/// Пользовательская форма
/// </summary>
public class UserForm : BaseTitleModel
{
    public List<RoleClient>? RoleClients { get; set; }
}
