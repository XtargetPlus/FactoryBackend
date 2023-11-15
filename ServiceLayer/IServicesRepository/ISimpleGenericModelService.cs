using DatabaseLayer.Helper;
using DB.Model;

namespace ServiceLayer.IServicesRepository;

public interface ISimpleGenericModelService<TEntity> : IErrorsMapper, IDisposable
    where TEntity : class
{
    Task<int> AddAsync(TEntity entity);
    Task ChangeAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<List<BaseTitleModel>> GetAllAsync(string text = "");
    Task<BaseTitleModel?> GetFirstAsync(int id);
}
