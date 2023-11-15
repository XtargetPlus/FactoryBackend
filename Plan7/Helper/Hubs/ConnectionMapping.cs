namespace Plan7.Helper.Hubs;

/// <summary>
/// Информация о подключенных пользователях к хабу
/// </summary>
public class ConnectionMapping
{
    /// <summary>
    /// Список подключений
    /// </summary>
    private readonly Dictionary<string, string> _connections = new();

    /// <summary>
    /// Количество подключенных пользователей
    /// </summary>
    public int Count { get { return _connections.Count; } }

    /// <summary>
    /// Добавление пользователя в список подключенных пользователей
    /// </summary>
    /// <param name="connectionId">Id подключения пользователя</param>
    /// <param name="user">Его Id подключения</param>
    public void Add(string connectionId, string user)
    {
        lock (_connections)
        {
            if (_connections.ContainsKey(connectionId))
                return;
            _connections.Add(connectionId, user);
        }
    }

    /// <summary>
    /// Получем список подключений
    /// </summary>
    /// <param name="connectionId">Id подключения пользователя</param>
    /// <returns>Список подключений (Id подключений) или пустой список</returns>
    public string GetConnections(string connectionId)
    {
        if (_connections.TryGetValue(connectionId, out string? user))
            return user ?? string.Empty;
        return string.Empty;
    }

    /// <summary>
    /// Удаление пользователя из списка подключенных к хабу
    /// </summary>
    /// <param name="connectionId">Id подключения пользователя</param>
    public void Remove(string connectionId)
    {
        lock (_connections)
        {
            if (!_connections.ContainsKey(connectionId))
                return;
            _connections.Remove(connectionId);
        }
    }
}
