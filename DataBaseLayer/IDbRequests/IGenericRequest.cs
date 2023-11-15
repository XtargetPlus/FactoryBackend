using Microsoft.EntityFrameworkCore.Query;
using Shared.Enums;
using System.Linq.Expressions;

namespace DatabaseLayer.IDbRequests;

public interface IGenericRequest<TEntity> : IDisposable
    where TEntity : class
{
    TEntity? Add(TEntity? item);
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity?> FindFirstAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool addQueryFilters = true);
    Task<TResult?> FindFirstAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool addQueryFilters = true);
    Task<TEntity?> IncludeCollectionAsync<TOut>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TOut>>> expression,
        Func<IQueryable<TOut>, IIncludableQueryable<TOut, object>>? include = null,
        bool addQueryFilters = true)
        where TOut : class;
    void Remove(TEntity? entity);
    void Update(TEntity? item);
    void Update(IEnumerable<TEntity>? items);
    Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> properties);
    IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>> filter,
        TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
        bool distinct = false);
    Task<List<TEntity>?> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
        int skip = 0,
        int take = 0,
        bool distinct = false,
        bool addQueryFilters = true);
    Task<List<TResult>?> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
        int skip = 0,
        int take = 0,
        bool distinct = false,
        bool addQueryFilters = true);
}