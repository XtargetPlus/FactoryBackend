using DB.Model.UserInfo;

namespace DB.Model.StorageInfo.Graph;

/// <summary>
/// Доступ к операционным графикам сторонним сотрудникам
/// </summary>
public class OperationGraphUser
{
    /// <summary>
    /// Доступ только на чтение (true/false)
    /// </summary>
    public bool IsReadonly { get; set; }
    /// <summary>
    /// Для какого графика указываем пользователя
    /// </summary>
    public int OperationGraphId { get; set; }
    public OperationGraph? OperationGraph { get; set; }
    /// <summary>
    /// Пользователь, которого привязывания к графику
    /// </summary>
    public int UserId { get; set; }
    public User? User { get; set; }
}
