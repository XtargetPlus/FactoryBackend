using DB;
using DB.Model.UserInfo;
using ServiceLayer.IServicesRepository;
using DatabaseLayer.IDbRequests;
using DB.Model.UserInfo.RoleInfo;
using Shared.Dto;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.RoleS;

/// <summary>
/// Сервис ролей
/// </summary>
public class RoleService : ErrorsMapper, IRoleService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Role> _repository;

    public RoleService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="title">Наименование добавляемой записи</param>
    /// <returns>Id добавленной записи или null (ошибка и/или предупреждения)</returns>
    public async Task<int?> AddAsync(TitleDto dto)
    {
        var role = await _context.AddWithValidationsAndSaveAsync(new Role() { Title = dto.Title }, this);
        return role?.Id;
    }

    /// <summary>
    /// Редактирование записи
    /// </summary>
    /// <param name="dto">Информация ня редактирование</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task ChangeAsync(BaseDto dto)
    {
        _repository.Update(new Role() { Id = dto.Id, Title = dto.Title });
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Role { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем список ролей
    /// </summary>
    /// <param name="take">Сколько получить (по умолчанию передавать 0)</param>
    /// <param name="skip">Сколько пропустить (по умолчанию передавать 0)</param>
    /// <returns>Список ролей</returns>
    public async Task<List<BaseDto>?> GetAllAsync(string text) => await _repository.GetAllAsync<BaseDto>(filter: !string.IsNullOrEmpty(text) ? (r => r.Title.Contains(text)) : null);

    /// <summary>
    /// Получаем весь список наименований ролей
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>?> GetAllTitlesAsync() => await _repository.GetAllAsync(r => r.Title);

    public void Dispose() => _context.Dispose();
}
