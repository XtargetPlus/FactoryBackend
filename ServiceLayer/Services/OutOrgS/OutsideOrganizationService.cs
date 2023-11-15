using DB;
using DB.Model.AccessoryInfo;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IOutsideOrganizationCountServices;
using Shared.Dto;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.OutOrgS;

/// <summary>
/// Сервис сторонних организаций
/// </summary>
public class OutsideOrganizationService : ErrorsMapper, IOutsideOrganizationService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OutsideOrganization> _repository;

    public OutsideOrganizationService(DbApplicationContext context, IMapper dataMapper)
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
        var outsideOrganization = await _context.AddWithValidationsAndSaveAsync(new OutsideOrganization { Title = dto.Title }, this);
        return outsideOrganization?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(BaseDto dto)
    {
        var outsideOrganization = await _repository.FindByIdAsync(dto.Id);
        if (outsideOrganization is null)
        {
            AddErrors("Не удалось получить стороннюю организацию");
            return;
        }
        if (outsideOrganization.Title == dto.Title)
            return;

        outsideOrganization.Title = dto.Title;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new OutsideOrganization { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<BaseDto?> GetFirstAsync(int id)
    {
        var outsideOrganization = await _repository.FindFirstAsync<BaseDto>(filter: oo => oo.Id == id);
        if (outsideOrganization is null)
            AddErrors("Не удалось получить стороннюю организацию");
        return outsideOrganization;
    }

    /// <summary>
    /// Получаем список сторонних организаций
    /// </summary>
    /// <param name="take">Сколько получить</param>
    /// <param name="skip">Сколько пропустить</param>
    /// <returns>Количество сторонних организаций</returns>
    public async Task<List<BaseDto>?> GetAllAsync(int take, int skip) => 
        await _repository.GetAllAsync<BaseDto>(
            orderBy: o => o.OrderBy(oo => oo.Id),
            take: take,
            skip: skip);

    public void Dispose() => _context.Dispose();  
}
