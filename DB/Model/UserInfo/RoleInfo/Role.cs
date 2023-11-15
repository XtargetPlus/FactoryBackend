using System.ComponentModel.DataAnnotations;

namespace DB.Model.UserInfo.RoleInfo;

/// <summary>
/// Роль
/// </summary>
public class Role : BaseModel
{
    /// <summary>
    /// Наименование
    /// </summary>
    [MinLength(1, ErrorMessage = "Длина строки минимум 1 символ")]
    [MaxLength(100, ErrorMessage = "Длина строки максимум 100 символов")]
    public string Title { get; set; } = null!;
    public List<RoleClient>? RoleClients { get; set; }
    public List<User>? Users { get; set; }
}
