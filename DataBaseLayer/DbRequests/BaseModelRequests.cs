using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.BasicStructuresExtensions;
using Shared.Enums;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace DatabaseLayer.IDbRequests;

/// <summary>
/// Базовый Generic класс для обращения к базе данных
/// </summary>
/// <typeparam name="TEntity">Класс модели, к которой будут происходить обращения</typeparam>
public class BaseModelRequests<TEntity> : IGenericRequest<TEntity>
	where TEntity : class
{
	readonly DbContext _context;
	readonly DbSet<TEntity> _dbSet;
	private readonly IMapper _dataMapper;

	/// <summary>
	/// Предупреждения
	/// </summary>
	public ImmutableList<ValidationResult>? Warnings { get; set; }
	public bool HasWarnings => Warnings != null && Warnings.Any();

	public BaseModelRequests(DbContext db, IMapper dataMapper)
	{
		_context = db;
		_dbSet = _context.Set<TEntity>();
		Warnings = null;
		_dataMapper = dataMapper;
	}

	public async Task<bool> IsNotThereRecord(int id)
	{
		return await _dbSet.FindAsync(id) != null;
	}

	/// <summary>
	/// Добавление записи в базу
	/// </summary>
	/// <param name="entity">Класс модели</param>
	/// <returns>Добавленную в базу запись с Id или null</returns>
	public TEntity? Add(TEntity? entity)
	{
		if (entity == null)
			return null;

		_context.Entry(entity).State = EntityState.Added;
		return entity;
	}

	/// <summary>
	/// Ищет запись по Id
	/// </summary>
	/// <param name="id">Id записи в базе</param>
	/// <returns>Возвращает запись или null</returns>
	public async Task<TEntity?> FindByIdAsync(int id) => await _dbSet.FindAsync(id);

	/// <summary>
	/// Ищет первую запись подходящую под условие
	/// </summary>
	/// <param name="filter">Условие поиска</param>
	/// <param name="include">Дополнительные данные для включения</param>
	/// <returns>Найденный элемент в базе или null</returns>
	public async Task<TEntity?> FindFirstAsync(
		Expression<Func<TEntity, bool>> filter,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = _dbSet.Where(filter).AsQueryable();
			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (include != null)
				_resetSet = include(_resetSet);
			var _result = await _resetSet.FirstOrDefaultAsync();

			return _result;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// Ищет первую запись подходящую под условие
	/// </summary>
	/// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
	/// <param name="select">Какие данные и в каком виде должны вернуться</param>
	/// <param name="filter">Условия выборки</param>
	/// <param name="include">Дополнительные данные для включения</param>
	/// <returns>Найденный объект типа TResult или null</returns>
	public async Task<TResult?> FindFirstAsync<TResult>(
		Expression<Func<TEntity, TResult>> select,
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = _dbSet.AsQueryable();
			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (filter != null)
				_resetSet = _resetSet.Where(filter);
			if (include != null)
				_resetSet = include(_resetSet);
			var _result = await _resetSet.Select(select).FirstOrDefaultAsync();

			return _result;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return default;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntityDto"></typeparam>
	/// <param name="filter"></param>
	/// <param name="include"></param>
	/// <param name="addQueryFilters"></param>
	/// <returns></returns>
	public async Task<TEntityDto?> FindFirstAsync<TEntityDto>(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = _dbSet.AsQueryable();
			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (filter != null)
				_resetSet = _resetSet.Where(filter);
			if (include != null)
				_resetSet = include(_resetSet);
				
			var _result = await _resetSet.ProjectTo<TEntityDto>(_dataMapper.ConfigurationProvider).FirstOrDefaultAsync();

			return _result;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return default;
	}

	/// <summary>
	/// Синхронно ищет первый объект по условию
	/// </summary>
	/// <param name="filter">Условие выборки</param>
	/// <returns>Найденный объект или null</returns>
	public TEntity? FindFirst(Expression<Func<TEntity, bool>> filter)
	{
		try
		{
			var _result = _dbSet.Where(filter).FirstOrDefault();
			return _result;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// Синхронно ищет все элементы по условию
	/// </summary>
	/// <param name="filter">Условие выборки</param>
	/// <param name="trackingOptions">Фильтр отслеживания найденных объектов, по умолчанию не отслеживаются</param>
	/// <param name="distinct">Уникальные записи, по умолчанию нет</param>
	/// <returns>Список найденных записей или null</returns>
	public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null,
		TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
		bool distinct = false)
	{
		var _resetSet = _dbSet.AsQueryable();
		if (filter != null)
			_resetSet = _resetSet.Where(filter);

		_resetSet = BaseModelRequests<TEntity>.GetQueryTracking(_resetSet, trackingOptions);

		if (distinct)
			_resetSet = _resetSet.Distinct();
		return _resetSet;
	}

	/// <summary>
	/// Поиск элементов по определенным критериям
	/// </summary>
	/// <param name="filter">Условие выборки</param>
	/// <param name="orderBy">Группировка по чему либо</param>
	/// <param name="include">Дополнительные данные для включения</param>
	/// <param name="trackingOptions">Фильтр отслеживания найденных объектов, по умолчанию не отслеживаются</param>
	/// <param name="skip">Сколько пропустить</param>
	/// <param name="take">Сколько получить</param>
	/// <param name="distinct">Уникальные или нет, по умолчанию false</param>
	/// <returns>Список найденных записей или null</returns>
	public async Task<List<TEntity>?> GetAllAsync(
		Expression<Func<TEntity, bool>>? filter = null, 
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
		int skip = 0, 
		int take = 0,
		bool distinct = false,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = _dbSet.AsQueryable();
			if (filter != null)
				_resetSet = _resetSet.Where(filter);

			_resetSet = GetQueryTracking(_resetSet, trackingOptions);

			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (distinct)
				_resetSet = _resetSet.Distinct();
			if (include != null)
				_resetSet = include(_resetSet);
			if (orderBy != null)
				_resetSet = orderBy(_resetSet);
			if (take > 0)
				_resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

			return await _resetSet.ToListAsync();
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// Поиск элементов по определенным критериям
	/// </summary>
	/// <typeparam name="TResult">Тип данных результирующей выборки</typeparam>
	/// <param name="select">Какие данные и в каком виде должны вернуться</param>
	/// <param name="filter">Условие выборки</param>
	/// <param name="orderBy">Группировка по чему либо</param>
	/// <param name="include">Дополнительные данные для включения</param>
	/// <param name="trackingOptions">Фильтр отслеживания найденных объектов, по умолчанию не отслеживаются</param>
	/// <param name="skip">Сколько пропустить</param>
	/// <param name="take">Сколько получить</param>
	/// <param name="distinct">Уникальные или нет, по умолчанию false</param>
	/// <returns>Список найденный записей приведенных к типу TResult или null</returns>
	public async Task<List<TResult>?> GetAllAsync<TResult>(
		Expression<Func<TEntity, TResult>> select,
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
		int skip = 0,
		int take = 0,
		bool distinct = false,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = filter != null ? 
				_dbSet.Where(filter).AsQueryable() : 
				_dbSet.AsQueryable();

			_resetSet = BaseModelRequests<TEntity>.GetQueryTracking(_resetSet, trackingOptions);

			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (include != null)
				_resetSet = include(_resetSet);
			if (orderBy != null)
				_resetSet = orderBy(_resetSet);
			if (take > 0)
				_resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

			var _result = _resetSet.Select(select).AsQueryable();

			if (distinct)
				_result = _result.Distinct();

			return await _result.ToListAsync();
		}
			
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntityDto"></typeparam>
	/// <param name="filter"></param>
	/// <param name="orderBy"></param>
	/// <param name="include"></param>
	/// <param name="trackingOptions"></param>
	/// <param name="skip"></param>
	/// <param name="take"></param>
	/// <param name="distinct"></param>
	/// <param name="addQueryFilters"></param>
	/// <returns></returns>
	public async Task<List<TEntityDto>?> GetAllAsync<TEntityDto>(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		TrackingOptions trackingOptions = TrackingOptions.AsNoTracking,
		int skip = 0,
		int take = 0,
		bool distinct = false,
		bool addQueryFilters = true)
	{
		try
		{
			var _resetSet = filter != null ?
				_dbSet.Where(filter).AsQueryable() :
				_dbSet.AsQueryable();

			_resetSet = GetQueryTracking(_resetSet, trackingOptions);

			if (!addQueryFilters)
				_resetSet = _resetSet.IgnoreQueryFilters();
			if (include != null)
				_resetSet = include(_resetSet);
			if (orderBy != null)
				_resetSet = orderBy(_resetSet);
			if (take > 0)
				_resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

			var _result = _resetSet.ProjectTo<TEntityDto>(_dataMapper.ConfigurationProvider).AsQueryable();

			if (distinct)
				_result = _result.Distinct();

			return await _result.ToListAsync();
		}

		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// Догружаем коллекцию
	/// </summary>
	/// <typeparam name="TOut">Тип данных, которые получатся при дозагрузки коллекции</typeparam>
	/// <param name="entity">Объект, переданный на дозагрузку</param>
	/// <param name="expression">Какие данные нужно подгрузить</param>
	/// <param name="include">Какие данные нужно дополнительно включить</param>
	/// <returns>Объект, с подзагруженными данными или null</returns>
	public async Task<TEntity?> IncludeCollectionAsync<TOut>(TEntity entity, 
		Expression<Func<TEntity, IEnumerable<TOut>>> expression,
		Func<IQueryable<TOut>, IIncludableQueryable<TOut, object>>? include = null,
		bool addQueryFilters = true)
		where TOut : class
	{
		try
		{
			var query = _context.Entry(entity).Collection(expression).Query();
			if (!addQueryFilters)
				query = query.IgnoreQueryFilters();

			if (include == null)
				await query.LoadAsync();
			else
				await include(query).LoadAsync();
			return entity;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="entity"></param>
	/// <param name="expression"></param>
	/// <returns></returns>
	public async Task<TEntity?> InculdeReferenceAsync<TProperty>(
		TEntity entity,
		Expression<Func<TEntity, TProperty?>> expression)
	where TProperty : class
	{
		try
		{
			await _context.Entry(entity).Reference(expression).LoadAsync();
			return entity;
		}
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"Ничего не найдено: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return null;
	}

	/// <summary>
	/// Удаляет элемент из базы данных
	/// </summary>
	/// <param name="entity">Элемент</param>
	/// <returns>Количество изменений или 0</returns>
	public void Remove(TEntity? entity)
	{
		if (entity == null)
			return;
		_context.Entry(entity).State = EntityState.Deleted;
	}

	/// <summary>
	/// Удаление коллекции
	/// </summary>
	/// <param name="items">Коллекция данные на удаление</param>
	/// <returns>Количество изменений или 0</returns>
	public void Remove(IEnumerable<TEntity>? items)
	{
		if (items.IsNullOrEmpty())
			return;
		foreach (var item in items!)
			_context.Entry(item).State = EntityState.Deleted;
	}

	/// <summary>
	/// Обновление записи
	/// </summary>
	/// <param name="entity">Запись на обновление</param>
	/// <returns>n > 0 если нет ошибок, 1 если есть HasWarnings == true, 0 если произошла ошибка</returns>
	public void Update(TEntity? entity)
	{
		if (entity == null)
			return;
		_context.Entry(entity).State = EntityState.Modified;
	}

	/// <summary>
	/// Обновление коллекции данных
	/// </summary>
	/// <param name="items">Коллекция данных</param>
	/// <returns>n > 0 если нет ошибок, 1 если есть HasWarnings == true, 0 если произошла ошибка</returns>
	public void Update(IEnumerable<TEntity>? items)
	{
		if (items.IsNullOrEmpty())
			return;
		foreach (var item in items!)
			_context.Entry(item).State = EntityState.Modified;
	}

	/// <summary>
	/// Обновление с определенных параметров записи по определенным фильтрам
	/// </summary>
	/// <param name="filter">Условие обновления</param>
	/// <param name="properties">Параметры, которые необходимо обновить</param>
	/// <returns>n > 0 если нет ошибок, 1 если есть HasWarnings == true, 0 если произошла ошибка</returns>
	public async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filter,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> properties)
	{
		try
		{
			return await _dbSet.Where(filter).ExecuteUpdateAsync(properties);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			Console.WriteLine($"Ошибка добавления (произошло параллельное обновление): {ex.Message}");
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"Ошибка добавления: {ex.Message}");
		}
		catch (OperationCanceledException ex)
		{
			Console.WriteLine($"Отмена операции: {ex.Message}");
		}
		return 0;
	}

	/// <summary>
	/// Установка отслеживания изменений
	/// </summary>
	/// <param name="entity">Запись, к которой добавится функция отслеживания изменений</param>
	/// <param name="trackingOptions">Функция отслеживания</param>
	/// <returns>Запись к которой добавлена функция остлеживания</returns>
	private static IQueryable<TEntity> GetQueryTracking(IQueryable<TEntity> entity, TrackingOptions trackingOptions) =>
		trackingOptions switch
		{
			TrackingOptions.AsNoTracking => entity.AsNoTracking(),
			TrackingOptions.AsNoTrackingWithIdentityResolution => entity.AsNoTrackingWithIdentityResolution(),
			_ => entity
		};

	public void Dispose() => _context.Dispose();
}