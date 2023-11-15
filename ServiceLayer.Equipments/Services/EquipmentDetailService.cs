using DatabaseLayer.IDbRequests;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB;
using DatabaseLayer.DbRequests.EquipmentToDb;
using Shared.Dto.Detail;
using Shared.Dto.Equipment.Filters;
using ServiceLayer.Equipments.Services.Interfaces;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.EquipmentS;

/// <summary>
/// Сервис деталей станков
/// </summary>
public class EquipmentDetailService : ErrorsMapper, IEquipmentDetailService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<EquipmentDetail> _repository;
    private readonly IMapper _dataMapper;

    public EquipmentDetailService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление детали к станку
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(BaseSerialTitleDto dto)
    {
        var equipmentDetail = await _context.AddWithValidationsAndSaveAsync(new EquipmentDetail { Title = dto.Title, SerialNumber = dto.SerialNumber }, this);
        return equipmentDetail?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(BaseIdSerialTitleDto dto)
    {
        var equipmentDetail = await _repository.FindByIdAsync(dto.DetailId);
        if (equipmentDetail is null)
        {
            AddErrors("Не удалось получить деталь станка");
            return;
        }

        equipmentDetail.Title = dto.Title;
        equipmentDetail.SerialNumber = dto.SerialNumber;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new EquipmentDetail { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем список деталей станков с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список деталей станков</returns>
    public async Task<List<BaseIdSerialTitleDto>?> GetAllAsync(GetAllEquipmentDetailFilters filters) =>
        await new EquipmentDetailRequests(_context, _dataMapper).GetAllAsync(filters);

    /// <summary>
    /// Получение списка деталей определенного станка с учетом фильтров
    /// </summary>
    /// <param name="filters">Фильтры</param>
    /// <returns>Список деталей станка или ошибка, если id станка <= 0 </returns>
    public async Task<List<BaseIdSerialTitleDto>?> GetAllFromEquipmentAsync(GetAllEquipmentDetailsFromEquipmentFilters filters) =>
        await new EquipmentDetailRequests(_context, _dataMapper).GetAllFromEquipmentAsync(filters);

    public void Dispose() => _context.Dispose();
}
