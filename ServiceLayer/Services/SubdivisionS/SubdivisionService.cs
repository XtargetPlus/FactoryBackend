using DB;
using DB.Model.SubdivisionInfo;
using System.ComponentModel;
using ServiceLayer.IServicesRepository;
using DatabaseLayer.IDbRequests;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;
using Shared.Dto.Subdiv;
using BizLayer.Repositories.SubdivisionR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.SubdivisionS;

/// <summary>
/// Сервис подразделений
/// </summary>
public class SubdivisionService : ErrorsMapper, ISubdivisionService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Subdivision> _repository;

    public SubdivisionService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddValueAsync(BaseSubdivisionDto dto)
    {
        var subdivision = await _context.AddWithValidationsAndSaveAsync(new Subdivision() { Title = dto.Title, FatherId = dto.FatherId != 0 ? dto.FatherId : null }, this);
        return subdivision?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(BaseDto dto)
    {
        var subdivision = await SubdivisionReadWithValidations.GetAsync(_repository, dto.Id, this);
        if (subdivision is null)
            return;
        if (subdivision.Title == dto.Title)
            return;
        
        subdivision.Title = dto.Title;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new Subdivision { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="id">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<SubdivisionGetDto?> GetFirstAsync(int id)
    {
        var subdivision = await _repository.FindFirstAsync<SubdivisionGetDto>(include: i => i.Include(s => s.Subdivisions!));
        if (subdivision is null)
            AddErrors("Не удалось получить подразделение");
        return subdivision;
    } 

    /// <summary>
    /// Получаем список подразделений, принадлежащие определенному подразделению
    /// </summary>
    /// <param name="fatherId">Id подразделения, чьи подразделения мы хотим получить</param>
    /// <returns>Подразделения или null (ошибки)</returns>
    public async Task<List<SubdivisionGetDto>?> GetAllLevelAsync([DefaultValue(null)] int? fatherId)
    {
        var subdivisions = await _repository.GetAllAsync<SubdivisionGetDto>(filter: s => s.FatherId == fatherId, include: i => i.Include(s => s.Subdivisions!));
        if (subdivisions is null)
        {
            AddErrors("Не удалось получить список подразделения определенного уровня");
            return null;
        }
        return subdivisions.Count == 0 ? new() : subdivisions;
    }

    /// <summary>
    /// Получаем весь список подразделений с учетом родителей "пд1: пд2: пд3: пд4" 
    /// </summary>
    /// <returns>Список подразделений</returns>
    public async Task<List<BaseDto>?> GetAllAsync()
    {
        var subdivisions = await _repository.GetAllAsync<SubdivisionDto>();

        if (subdivisions is null)
        {
            AddErrors("Не удалось получить список подразделения");
            return null;
        }

        List<BaseDto> results = new();
        foreach (var subdivision in subdivisions)
        {
            results.Add(subdivision.FatherId is null
                ? new BaseDto { Id = subdivision.Id, Title = subdivision.Title }
                : new BaseDto
                {
                    Id = subdivision.Id,
                    Title = $"{results.Where(s => s.Id == subdivision.FatherId).Select(s => s.Title).First()}: {subdivision.Title}"
                });
        }

        return results;
    }

    /// <summary>
    /// Получаем весь список подразделений с учетом родителей "пд1: пд2: пд3: пд4 и наличию станков" 
    /// </summary>
    /// <returns>Список подразделений</returns>
    public async Task<List<BaseDto>?> GetAllByEquipmentContainAsync(bool isContainEquipments = false)
    {
        var result = !isContainEquipments 
                                        ? await _repository.GetAllAsync<BaseDto>(include: i => i.Include(s => s.Equipments!), filter: s => s.Equipments!.Count == 0)
                                        : await _repository.GetAllAsync<BaseDto>(include: i => i.Include(s => s.Equipments!), filter: s => s.Equipments!.Count > 0);
        if (result is not null) return result;
        
        AddErrors("Не удалось получить список подразделения");
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tpId"></param>
    /// <returns></returns>
    public async Task<List<BaseDto>?> GetAllWithoutTechProcessAsync(int tpId)
    {
        var results = await _repository.GetAllAsync<BaseDto>(filter: s => s.TechnologicalProcessStatuses!.All(tps => tps.TechnologicalProcessId != tpId));
        if (results is null)
            AddErrors("Не удалось выгрузить список подразделений");
        return results;
    }

    public void Dispose() => _context.Dispose();  
}
