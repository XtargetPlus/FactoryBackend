namespace ServiceLayer.IRepository;

/// <summary>
/// Базовые интерфейс сервиса для сервисов по типу DetailType
/// </summary>
/// <typeparam name="T">Класс</typeparam>
public interface IBaseModelRepository<T> : IDisposable
    where T : class
{
    Task<int?> AddAsync(string value); 
    Task<int?> ChangeAsync(T baseRequest);
    Task<int?> DeleteAsync(int id);
    Task<List<T>?> GetAllAsync();
}
