using DB;
using DB.Model.UserInfo;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IProfessionServices;
using Shared.Dto;
using Shared.Dto.Profession.Filters;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.ProfessionS;

/// <summary>
/// Сервис профессий
/// </summary>
public class ProfessionService : ErrorsMapper, IProfessionService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Profession> _repository;

    public ProfessionService(DbApplicationContext context, IMapper dataMapper)
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
        var profession = await _context.AddWithValidationsAndSaveAsync(new Profession { Title = dto.Title }, this);
        return profession?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(BaseDto dto)
    {
        var profession = await _repository.FindByIdAsync(dto.Id);
        if (profession is null)
        {
            AddErrors("Не удалось получить профессию");
            return;
        }
        if (profession.Title == dto.Title)
            return;

        profession.Title = dto.Title;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Profession { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<BaseDto?> GetFirstAsync(int id)
    {
        var profession = await _repository.FindFirstAsync<BaseDto>(filter: p => p.Id == id);
        if (profession is null)
            AddErrors("Не удалось найти профессию");
        return profession;
    }

    /// <summary>
    /// Получаем список профессий с пагинацией
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список профессий</returns>
    public async Task<List<BaseDto>?> GetAllAsync(GetAllProfessionFilters filters) =>
        await _repository.GetAllAsync<BaseDto>(
            filter: !string.IsNullOrEmpty(filters.Text) ? (p => p.Title.Contains(filters.Text)) : null,
            orderBy: o => o.OrderBy(p => p.Title),
            take: filters.Take,
            skip: filters.Skip);

    public void Dispose() => _context.Dispose();
}
