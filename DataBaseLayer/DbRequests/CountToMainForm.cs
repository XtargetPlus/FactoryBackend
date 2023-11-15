using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DatabaseLayer.IDbRequests;

/// <summary>
/// Считает количество
/// </summary>
/// <typeparam name="TEntity">Модель базы данных</typeparam>
public class CountToMainForm<TEntity>: ICountInformationDb<TEntity>
    where TEntity : class
{
    private readonly DbContext _db;
    private readonly DbSet<TEntity> _dbSet;

    public CountToMainForm(DbContext db)
    {
        _db = db;
        _dbSet = _db.Set<TEntity>();    
    }

    /// <summary>
    /// Считывает количество
    /// </summary>
    /// <returns>Количество записей</returns>
    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    /// <summary>
    /// Считывает количество
    /// </summary>
    /// <param name="filter">Условие при считывании</param>
    /// <returns>Количество записей</returns>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, bool addQueryFilters = true) => 
        addQueryFilters ? await _dbSet.Where(filter).CountAsync() : await _dbSet.IgnoreQueryFilters().Where(filter).CountAsync();

    /// <summary>
    /// Считывает количество
    /// </summary>
    /// <param name="filter">Условие при считывании</param>
    /// <param name="include">Данные для включения</param>
    /// <returns>Количество</returns>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include) =>
        await include(_dbSet.Where(filter)).CountAsync();

    public void Dispose() => _db.Dispose();
}