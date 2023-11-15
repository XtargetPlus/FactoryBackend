using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.TechnologicalProcessInfo;
using ServiceLayer.IServicesRepository.IOperationServices;
using DatabaseLayer.DbRequests.OperationToDb;
using Shared.Dto.Operation;
using Shared.Dto.Operation.Filters;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.OperationS;

/// <summary>
/// Сервис операций
/// </summary>
public class OperationService : ErrorsMapper, IOperationService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Operation> _repository;
    private readonly IMapper _dataMapper;

    public OperationService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    public async Task<int?> AddAsync(BaseOperationDto dto)
    {
        var operation = await _context.AddWithValidationsAndSaveAsync(new Operation { ShortName = dto.ShortName, FullName = dto.FullName }, this);
        return operation?.Id;
    }

    public async Task ChangeAsync(OperationDto dto)
    {
        var operation = await _repository.FindByIdAsync(dto.Id);
        if (operation is null)
        {
            AddErrors("Не удалось получить операцию");
            return;
        }

        operation.ShortName = dto.ShortName;
        operation.FullName = dto.FullName;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Operation { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<OperationDto?> GetFirstAsync(int id)
    {
        var operation = await _repository.FindFirstAsync<OperationDto>(filter: o => o.Id == id);
        if (operation is null)
            AddErrors("Не удалось получить операцию");
        return operation;
    }

    /// <summary>
    /// Получение списка операций для селектов
    /// </summary>
    /// <returns>Список операций</returns>
    public async Task<List<OperationDto>?> GetAllForChoiceAsync() =>
        await _repository.GetAllAsync<OperationDto>(take: 0);

    /// <summary>
    /// Получаем список операций с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список операций с учетом фильтров</returns>
    public async Task<List<OperationDto>?> GetAllAsync(GetAllOperationFilters filters) => await new OperationRequests(_context).GetAllAsync(filters, _dataMapper);

    public void Dispose() => _context.Dispose();
}
