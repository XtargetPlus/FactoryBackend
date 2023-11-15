using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DatabaseLayer.IDbRequests;

public interface ICountInformationDb<TEntity> : IDisposable
    where TEntity : class
{
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, bool addQueryFilters = true);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
}