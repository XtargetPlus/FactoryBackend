using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model;
using ServiceLayer.IServicesRepository;

namespace ServiceLayer.Services.BaseS;

public class SimpleGenericModelService<TEntity> : ErrorsMapper, ISimpleGenericModelService<TEntity>
    where TEntity : class
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TEntity> _repository;

    public SimpleGenericModelService(DbApplicationContext context)
    {
        _context = context;
        _repository = new(_context, null);
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        var result = await _context.AddWithValidationsAndSaveAsync(entity, this);
        return (int)(result?.GetType().GetProperty("Id")?.GetValue(result) ?? 0);
    }

    public async Task ChangeAsync(TEntity entity)
    {
        _repository.Update(entity);
        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _repository.Remove(entity);
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    public async Task<List<BaseTitleModel>> GetAllAsync(string text = "")
    {
        List<BaseTitleModel> result = new();
        var isSearchTextIsNullOrEmpty = string.IsNullOrEmpty(text);
        foreach (var genericEntity in _repository.GetAll())
        {
            if (genericEntity is not BaseTitleModel entity)
                continue;
            if (!isSearchTextIsNullOrEmpty)
            {
                if (entity.Title.ToLower().Contains(text.ToLower()))
                    result.Add(entity);
                continue;
            }
            result.Add(entity);
        }
        return result;
    }

    public async Task<BaseTitleModel?> GetFirstAsync(int id)
    {
        if (await _repository.FindByIdAsync(id) is BaseTitleModel entity)
            return entity;
        return null;
    }

    public void Dispose() => _context.Dispose();
}
