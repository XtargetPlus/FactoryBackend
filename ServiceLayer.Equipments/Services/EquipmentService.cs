using DB;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DatabaseLayer.IDbRequests;
using DatabaseLayer.IDbRequests.EquipmentToDb;
using Shared.Dto.Equipment;
using Shared.Dto.Equipment.Filters;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.Equipments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Shared.Dto.Tools;
using DB.Model.ToolInfo;

namespace ServiceLayer.Services.EquipmentS;

/// <summary>
/// Сервис станков
/// </summary>
public class EquipmentService : ErrorsMapper, IEquipmentService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Equipment> _repository;
    private readonly IMapper _dataMapper;

    public EquipmentService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(AddEquipmentDto dto)
    {
        Equipment? equipment = new()
        {
            Title = dto.Title,
            SerialNumber = dto.SerialNumber,
            SubdivisionId = dto.SubdivisionId
        };
        equipment = await _context.AddWithValidationsAndSaveAsync(equipment, this);
        return equipment?.Id;
    }

    /// <summary>
    /// Добавление детали к станку
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns></returns>
    public async Task AddDetailAsync(EquipmentWithDetailDto dto)
    {
        var equipment = await _repository.FindByIdAsync(dto.EquipmentId);
        if (equipment is null)
            AddErrors("Не удалось получить станок");
        if (HasErrors)
            return;
        equipment!.EquipmentDetailContents = new() { new() { EquipmentDetailId = dto.EquipmentDetailId } };
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns></returns>
    public async Task ChangeAsync(ChangeEquipmentDto dto)
    {
        var equipment = await _repository.FindByIdAsync(dto.Id);
        if (equipment is null)
        {
            AddErrors("Не удалось получить станок");
            return;
        }

        if (dto.Title is not null && equipment.Title != dto.Title) 
            equipment.Title = dto.Title;
        if (dto.SerialNumber is not null && equipment.SerialNumber != dto.SerialNumber) 
            equipment.SerialNumber = dto.SerialNumber;
        if (dto.SubdivisionId != 0 && equipment.SubdivisionId != dto.SubdivisionId)
            equipment.SubdivisionId = dto.SubdivisionId;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Equipment { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Удаление детали из станка
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns></returns>
    public async Task DeleteDetailAsync(EquipmentWithDetailDto dto)
    {
        var equipment = await _repository.FindFirstAsync(
            filter: e => e.Id == dto.EquipmentId,
            include: i => i.Include(e => e.EquipmentDetailContents!.Where(ed => ed.EquipmentDetailId == dto.EquipmentDetailId)));
        if (equipment is null)
            AddErrors("Не удалось получить станок");
        if (!equipment?.EquipmentDetailContents?.Any() ?? true)
            AddErrors("Не удалось найти делать");
        if (HasErrors)
            return;

        equipment!.EquipmentDetailContents!.Remove(new EquipmentDetailContent() { EquipmentDetailId = dto.EquipmentDetailId, EquipmentId = dto.EquipmentId });

        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Ids
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<GetEquipmentDto?> GetFirstAsync(int id)
    {
        var equipment = await _repository.FindFirstAsync<GetEquipmentDto>(filter: e => e.Id == id);
        if (equipment is null)
            AddErrors("Не удалось получить станок");
        return equipment;
    }

    //public async Task<List<GetToolDto>?> GetToolFromEquipmentAsync(int equipmentId)
    //{
    //    var tools = await new BaseModelRequests<EquipmentTool>(_context, _dataMapper)
    //        .GetAllAsync(
    //            filter: e => e.EquipmentId == equipmentId, 
    //            include: i => i.Include(e => e.Tool));
        
    //    List<GetToolDto>? result = new();
    //    foreach(var item in tools)
    //    {
    //        result.Add(new GetToolDto
    //        {
    //            Id = item.ToolId,
    //            Title = item.Tool?.Title,
    //            SerialNumber = item.Tool?.SerialNumber,
    //            Note = item.Tool?.Note
    //        });
    //    }
    //    return result;
        
    //}

    /// <summary>
    /// Получаем список станков с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список станков</returns>
    public async Task<List<GetEquipmentDto>?> GetAllAsync(GetAllEquipmentFilters filters) =>
        await new EquipmentRequests(_context, _dataMapper).GetAllAsync(filters);

    /// <summary>
    /// Получение списка станков по подразделению
    /// </summary>
    /// <param name="subdivisionId">Id подразделения</param>
    /// <returns>Список станков</returns>
    public async Task<List<GetEquipmentDto>?> GetAllBySubdivisionAsync(int subdivisionId) =>
        await _repository.GetAllAsync<GetEquipmentDto>(filter: e => e.SubdivisionId == subdivisionId);

    public void Dispose() => _context.Dispose();

   
}
