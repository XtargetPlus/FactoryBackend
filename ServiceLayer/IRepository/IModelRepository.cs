namespace ServiceLayer.IRepository;

// Старые интерфейсы, почти не используются

public interface IModelRepository<T> : IDisposable
    where T : class
{
    Task<int?> AddAsync(T value);
    Task<int?> ChangeAsync(T value);
    Task<List<T>?> GetAllAsync(int take, int skip);
    Task<int?> DeleteAsync(int id);
}

public interface IModelRepository<T1, T2> : IDisposable
    where T1 : class
    where T2 : class
{
    Task<int?> AddAsync(T1 value);
    Task<int?> ChangeAsync(T2 value);
    Task<List<T2>?> GetAllAsync(int take, int skip);
    Task<int?> DeleteAsync(int id);
}

public interface IModelRepository<T1, T2, TOrder, TKindOfOrder> : IDisposable
    where T1 : class
    where T2 : class
{
    Task<int?> AddAsync(T1 value);
    Task<int?> ChangeAsync(T2 value);
    Task<List<T2>?> GetAllAsync(int take, int skip, TOrder order, TKindOfOrder kindOfOrder);
    Task<int?> DeleteAsync(int id);
}

public interface IModelRepository<T1, T2, T3> : IDisposable
    where T1 : class
    where T2 : class
    where T3 : class
{
    Task<int?> AddAsync(T1 value);
    Task<int?> ChangeAsync(T2 value);
    Task<List<T3>?> GetAllAsync(int take, int skip);
    Task<int?> DeleteAsync(int id);
}

public interface IModelRepository<T1, T2, T3, TOrder, TKindOfOrder> : IDisposable
    where T1 : class
    where T2 : class
    where T3 : class
{
    Task<int?> AddAsync(T1 value);
    Task<int?> ChangeAsync(T2 value);
    Task<List<T3>?> GetAllAsync(int take, int skip, TOrder order, TKindOfOrder kindOfOrder);
    Task<int?> DeleteAsync(int id);
}
