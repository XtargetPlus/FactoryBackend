using DB;
using DB.Model.ProductInfo;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IClientServices;
using Shared.Dto;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.ClientS;

/// <summary>
/// Сервис клиентов (заказчиков)
/// </summary>
public class ClientService : ErrorsMapper, IClientService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Client> _repository;

    public ClientService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="dto">Наименование добавляемой записи</param>
    /// <returns>Id добавленной записи или null (ошибка и/или предупреждения)</returns>
    public async Task<int?> AddAsync(TitleDto dto)
    {
        var client = await _context.AddWithValidationsAndSaveAsync(new Client { Title = dto.Title }, this);
        return client?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(BaseDto dto)
    {
        var client = await _repository.FindByIdAsync(dto.Id);
        if (client is null)
        {
            AddErrors("Не удалось получить клиента");
            return;
        }
        if (client.Title == dto.Title)
            return;

        client.Title = dto.Title;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Client { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);

    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<BaseDto?> GetFirstAsync(int id)
    {
        var client = await _repository.FindFirstAsync<BaseDto>(filter: c => c.Id == id);
        if (client is null)
            AddErrors("Не удалось получить клиента");
        return client;
    } 

    /// <summary>
    /// Получаем список клиентов
    /// </summary>
    /// <param name="take">Сколько получить</param>
    /// <param name="skip">Сколько пропустить</param>
    /// <returns>Список клиентов</returns>
    public async Task<List<BaseDto>?> GetAllAsync(int take = 50, int skip = 0) =>
        await _repository.GetAllAsync<BaseDto>(
            orderBy: o => o.OrderBy(c => c.Id),
            take: take,
            skip: skip);

    public void Dispose() => _context.Dispose();
}
