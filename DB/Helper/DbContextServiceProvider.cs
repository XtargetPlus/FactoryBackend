using Microsoft.EntityFrameworkCore;

namespace DB.Helper;

/// <summary>
/// Сервис для передачи актуального контекста базы данных
/// </summary>
public class DbContextServiceProvider : IServiceProvider
{
    private readonly DbContext _currentContext;

    public DbContextServiceProvider(DbContext currentContext)
    {
        _currentContext = currentContext;
    }

    /// <summary>
    /// Получаем сервис
    /// </summary>
    /// <param name="serviceType">Передаем тип сервиса, который нужно получить</param>
    /// <returns>Если тип равен DbContext возвращаем currentContext или null</returns>
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(DbContext))
            return _currentContext;
        return null;    
    }
}
