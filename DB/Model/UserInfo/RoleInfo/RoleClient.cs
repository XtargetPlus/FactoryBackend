using DB.Model.UserInfo.RoleInfo;

namespace DB.Model.UserInfo;

/// <summary>
/// Функционал роли в форме
/// </summary>
public class RoleClient
{
    /// <summary>
    /// Форма (html)
    /// </summary>
    public int UserFormId { get; set; }
    public UserForm? UserForm { get; set; }
    /// <summary>
    /// Роль
    /// </summary>
    public int RoleId { get; set; }
    public Role? Role { get; set; } 
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Browsing { get; set; }
}