using DB;
using DB.Model.StatusInfo;
using ServiceLayer.IServicesRepository;
using DatabaseLayer.IDbRequests;
using Shared.Dto;
using Shared.Dto.Status;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.StatusS;

/// <summary>
/// Сервис статусов
/// </summary>
public class StatusService : ErrorsMapper, IStatusService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Status> _repository;

    public StatusService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(StatusChangeDto dto)
    {
        var status = await _context.AddWithValidationsAndSaveAsync(new Status { Title = dto.Title, TableName = dto.TableName }, this);
        return status?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(StatusDto dto)
    {
        if (await _repository.UpdateAsync(s => s.Id == dto.Id, s => s.SetProperty(s => s.Title, dto.Title)
                                                                     .SetProperty(s => s.TableName, dto.TableName)) < 1)
            AddErrors("Не удалось изменить статус");
        if (_repository.HasWarnings)
            AddWarnings(_repository.Warnings);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Status { Id = id });
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<StatusDto?> GetFirstAsync(int id)
    {
        var status = await _repository.FindFirstAsync<StatusDto>(filter: s => s.Id == id);
        if (status is null)
            AddErrors("Не удалось получить статус");
        return status;
    }

    /// <summary>
    /// Получаем список статусов
    /// </summary>
    /// <param name="take">Сколько получить (по умолчанию передавать 0)</param>
    /// <param name="skip">Сколько пропустить (по умолчанию передавать 0)</param>
    /// <returns>Список статусов</returns>
    public async Task<List<StatusDto>?> GetAllAsync(int take = 0, int skip = 0) => await _repository.GetAllAsync<StatusDto>();

    /// <summary>
    /// Получаем список статусов определенной таблицы
    /// </summary>
    /// <param name="tableName">Таблица</param>
    /// <returns>Список статусов</returns>
    public async Task<List<BaseDto>?> GetTableStatusesAsync(string tableName) => await _repository.GetAllAsync<BaseDto>(filter: s => s.TableName == tableName);

    public void Dispose() => _context.Dispose();
}
