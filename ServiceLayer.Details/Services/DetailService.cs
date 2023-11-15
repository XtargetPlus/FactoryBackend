using DB;
using DB.Model.DetailInfo;
using DatabaseLayer.IDbRequests.DetailToDb;
using DatabaseLayer.IDbRequests;
using Shared.Dto.Detail;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail.Filters;
using Shared.Dto;
using BizLayer.Repositories.DetailR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.Details.Services.Interfaces;
using AutoMapper;

namespace ServiceLayer.Details.Services;

/// <summary>
/// Сервис деталей
/// </summary>
public class DetailService : ErrorsMapper, IDetailService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Detail> _repository;
    private readonly IMapper _dataMapper;

    public DetailService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление детали
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(DetailMoreInfoDto dto)
    {
        Detail? detail = new()
        {
            Title = dto.Title,
            SerialNumber = dto.SerialNumber,
            Weight = dto.Weight,
            DetailTypeId = dto.DetailTypeId,
            UnitId = dto.UnitId
        };
        detail = await _context.AddWithValidationsAndSaveAsync(detail, this);
        return detail?.Id;
    }

    /// <summary>
    /// Изменение записи
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns></returns>
    public async Task ChangeAsync(DetailChangeDto dto)
    {
        var detail = await DetailSimpleRead.GetAsync(_repository, dto.Id, this);
        if (HasErrors)
            return;

        if (detail!.Title != dto.Title && dto.Title is not null) 
            detail.Title = dto.Title;
        if (detail.SerialNumber != dto.SerialNumber && dto.SerialNumber is not null) 
            detail.SerialNumber = dto.SerialNumber;
        if (detail.Weight != dto.Weight && dto.Weight >= 0) 
            detail.Weight = dto.Weight;
        if (detail.DetailTypeId != dto.DetailTypeId && dto.DetailTypeId > 0)
            detail.DetailTypeId = dto.DetailTypeId;
        if (detail.UnitId != dto.UnitId && dto.UnitId > 0)
            detail.UnitId = dto.UnitId;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detailsId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Dictionary<int, bool>?> IsCompositionsAsync(List<int> detailsId)
    {
        var details = await _repository.GetAllAsync(filter: d => detailsId.Contains(d.Id), include: i => i.Include(d => d.DetailsChildren));
        if (details is null)
        {
            AddErrors("Не удалось получить список деталей");
            return null;
        }

        Dictionary<int, bool> result = new();
        details.ForEach(d => result.Add(d.Id, d.DetailsChildren!.Any()));

        return result;
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="detailId">Id удаляемой записи</param>
    /// <returns></returns>
    public async Task DeleteAsync(int detailId)
    {
        _repository.Remove(new Detail { Id = detailId });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем запись по Id
    /// </summary>
    /// <param name="detailId">Id получаемой записи</param>
    /// <returns>Запись или null (ошибки)</returns>
    public async Task<GetDetailInfoWithIdDto?> GetFirstAsync(int detailId)
    {
        var detail = await _repository.FindFirstAsync<GetDetailInfoWithIdDto>(filter: d => d.Id == detailId);
        if (detail is null)
            AddErrors("Не получилось получить деталь");
        return detail;
    }

    /// <summary>
    /// Получаем список деталей с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список деталей</returns>
    public async Task<List<GetDetailDto>?> GetAllAsync(GetAllDetailFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId != 0)
            productDetailsId = (await new DetailService(_context, _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = false
            }) ?? new()).Select(d => d.DetailId).ToList();
        return await new DetailRequests(_context, _dataMapper).GetAllAsync(filters, productDetailsId);
    }

    /// <summary>
    /// Получаем список уникальных изделий, в которых применяется данная деталь с общим количеством деталей в этом изделии 
    /// </summary>
    /// <param name="detailId">Id детали, чей список изделий мы хотим получить</param>
    /// <returns></returns>
    public async Task<List<DetailProductsDto>?> GetAllProductsAsync(int detailId) =>
        await new DetailRepository(this, _dataMapper).GetAllProductsAsync(detailId, _context);

    /// <summary>
    /// Получаем весь состав изделия без повторяющихся деталей
    /// </summary>
    /// <param name="detailId">Id изделия</param>
    /// <returns></returns>
    public async Task<List<BaseIdSerialTitleDto>?> GetAllProductDetailsAsync(GetAllProductDetailsDto dto) =>
        await new DetailRepository(this, _dataMapper).GetAllProductDetailsAsync(dto, _context);

    /// <summary>
    /// Получаем подробную информацию о детали
    /// </summary>
    /// <param name="id">Id детали, чью информацию мы хотим получить</param>
    /// <returns>Информация о детали + список типов деталей + список единиц измерения или null (ошибки и/или предупреждения)</returns>
    public async Task<DetailListDto?> GetInfoAsync(int detailId)
    {
        DetailListDto? detailInfo = new()
        {
            DetailTypes = await new BaseModelRequests<DetailType>(_context, _dataMapper).GetAllAsync<BaseDto>(),
            Units = await new BaseModelRequests<Unit>(_context, _dataMapper).GetAllAsync<BaseDto>(),
        };
        if (detailInfo.DetailTypes is null || detailInfo.DetailTypes.Count <= 0)
            AddErrors("Не удалось получить типы деталей");
        if (detailInfo.Units is null || detailInfo.Units.Count <= 0)
            AddErrors("Не удалось получить единицы измерение");
        var detail = await DetailSimpleRead.GetAsync(_repository, detailId, this);

        if (HasErrors)
            return null;

        detailInfo.SerialNumber = detail!.SerialNumber;
        detailInfo.Title = detail.Title;
        detailInfo.Weight = detail.Weight;
        detailInfo.DetailTypeId = detail.DetailTypeId;
        detailInfo.UnitId = detail.UnitId;

        return detailInfo;
    }

    /// <summary>
    /// Получаем единицу измерения детали
    /// </summary>
    /// <param name="detailId">Id детали</param>
    /// <returns>Название единицы измерения</returns>
    public async Task<string?> GetDetailUnitAsync(int detailId)
    {
        var unit = await _repository.FindFirstAsync(select: d => d.Unit!.Title, filter: d => d.Id == detailId);
        if (unit is null)
            AddErrors("Не удалось получить единицу измерения детали");
        return unit;
    }

    public void Dispose() => _context.Dispose();
}
