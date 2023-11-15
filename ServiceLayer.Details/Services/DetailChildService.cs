using DatabaseLayer.IDbRequests;
using DB.Model.DetailInfo;
using DB;
using DatabaseLayer.IDbRequests.DetailToDb;
using Shared.Enums;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Detail.DetailChild.Filters;
using BizLayer.Repositories.DetailR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.Details.Services.Interfaces;
using AutoMapper;

namespace ServiceLayer.Details.Services;

/// <summary>
/// Сервис для составов деталей
/// </summary>
public class DetailChildService : ErrorsMapper, IDetailChildService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Detail> _detailRepository;
    private readonly BaseModelRequests<DetailsChild> _detailChildRepository;
    private readonly IMapper _dataMapper;

    public DetailChildService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _detailRepository = new(_context, dataMapper);
        _detailChildRepository = new(_context, dataMapper);
        _dataMapper = dataMapper;
    }

    /// <summary>
    /// Добавляем деталь в состав другой детали
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns></returns>
    public async Task AddAsync(DetailChildAddDto dto)
    {
        var detailsChildRepository = new DetailChildRepository(dto.ChildId, dto.FatherId, _context);

        if (dto.FatherId == dto.ChildId)
            AddErrors("Родительская и дочерняя детали сходятся");
        if (await detailsChildRepository.IsFatherDetailsSeniorAsync())
            AddErrors("Дочерняя деталь является деталью предком в составе");
        if (await detailsChildRepository.IsChildDetailInLineupAsync())
            AddErrors("Данная деталь уже находится в составе");

        if (HasErrors)
            return;

        var fatherDetail = await DetailSimpleRead.GetAsync(_detailRepository, dto.FatherId, this);
        var childDetail = await DetailSimpleRead.GetAsync(_detailRepository, dto.ChildId, this);
        if (HasErrors)
            return;

        DetailsChild detailsChild = new()
        {
            FatherId = fatherDetail!.Id,
            ChildId = childDetail!.Id,
            Count = dto.Count,
        };

        fatherDetail = await _detailRepository.IncludeCollectionAsync(fatherDetail, fd => fd.DetailsChildren!);
        detailsChild.Number = fatherDetail!.DetailsChildren!.Count + 1;

        await _context.AddWithValidationsAndSaveAsync(detailsChild, this);
    }

    /// <summary>
    /// Изменения поля в составе, меняется только количество
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns></returns>
    public async Task ChangeAsync(DetailChildAddDto dto)
    {
        var detailChild = await DetailChildSimpleRead.GetAsync(_detailChildRepository, dto.FatherId, dto.ChildId, this);
        if (HasErrors)
            return;
        if (detailChild!.Count == dto.Count)
            return;

        detailChild.Count = dto.Count;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка детали в составе между двумя другими с перерасчетом номеров
    /// </summary>
    /// <param name="dto">Данные для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenAsync(InsertBetweenChildDto dto)
    {
        if (dto.BeforeDetailNumber == dto.CurrentTargetDetailNumber)
        {
            AddErrors("Деталь №1 и деталь №2 одинаковы");
            return;
        }

        var isBeforeNumberIsNotZero = dto.BeforeDetailNumber != 0;

        var secondDetail = await DetailChildSimpleRead.GetByNumberAsync(_detailChildRepository, dto.FatherId, dto.CurrentTargetDetailNumber, this);
        if (HasErrors)
            return;

        var oldNumber = dto.CurrentTargetDetailNumber;
        var newNumber = isBeforeNumberIsNotZero ? dto.BeforeDetailNumber < oldNumber ? dto.BeforeDetailNumber + 1 : dto.BeforeDetailNumber : 1;

        var children = await _detailChildRepository.GetAllAsync(
                filter: dc => dc.FatherId == dto.FatherId
                        && dc.ChildId != secondDetail!.ChildId
                        && (oldNumber > newNumber ? dc.Number >= newNumber && dc.Number < oldNumber
                                                  : dc.Number <= newNumber && dc.Number > oldNumber),
                trackingOptions: TrackingOptions.WithTracking);
        if (children is null)
        {
            AddErrors("Не удалось получить список дочерних деталей больше детали №2");
            return;
        }

        children.ForEach(child => child.Number += oldNumber > newNumber ? 1 : -1);
        secondDetail!.Number = newNumber;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Меняем местами порядковые номера деталей в составе
    /// </summary>
    /// <param name="dto">Информация для свапа</param>
    /// <returns></returns>
    public async Task SwapChildrenNumbersAsync(DetailChildSwapDto dto)
    {
        var childFirst = await DetailChildSimpleRead.GetAsync(_detailChildRepository, dto.FatherId, dto.ChildFirstId, this);
        var childSecond = await DetailChildSimpleRead.GetAsync(_detailChildRepository, dto.FatherId, dto.ChildSecondId, this);
        if (HasErrors)
            return;

        (childFirst!.Number, childSecond!.Number) = (childSecond.Number, childFirst.Number);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление детали из состава
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns></returns>
    public async Task DeleteAsync(TwoDetailIdDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var number = await _detailChildRepository.FindFirstAsync(
            select: dc => dc.Number, 
            filter: dc => dc.FatherId == dto.FatherDetailId && dc.ChildId == dto.ChildDetailId);
        if (number is 0)
        {
            AddErrors("Не удалось получить порядковый номер детали");
            return;
        }

        _detailChildRepository.Remove(new DetailsChild { FatherId = dto.FatherDetailId, ChildId = dto.ChildDetailId });
        await _context.SaveChangesWithValidationsAsync(this, false);
        if (HasErrors || HasWarnings)
            return;

        /* 
         * если мы удаляем не последнюю деталь в составе, а например деталь с порядковым номером 1 и после нее есть детали с порядковым номером 2, 3 и тд 
         * уменьшаем всем порядковый номер на n -= 1
         */
        var children = await _detailChildRepository.GetAllAsync(
            filter: dc => dc.FatherId == dto.FatherDetailId && dc.Number > number,
            trackingOptions: TrackingOptions.WithTracking);
        if (children is { Count: > 0 })
        {
            children.ForEach(c => c.Number--);
            await _context.SaveChangesWithValidationsAsync(this);
        }
        if (HasErrors || HasWarnings)
            return;

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Получаем запись в составе
    /// </summary>
    /// <param name="fatherId">Id детали, в которой находится деталь</param>
    /// <param name="childId">Id детали, которая входит в состав</param>
    /// <returns>Запись в составе или null (ошибки)</returns>
    public async Task<GetDetailChildDto?> GetFirstChildAsync(int fatherId, int childId)
    {
        var detailChild = await _detailChildRepository.FindFirstAsync<GetDetailChildDto>(filter: dc => dc.ChildId == childId && dc.FatherId == fatherId);
        if (detailChild is null)
            AddErrors("Не удалось получить деталь состав");
        return detailChild;
    }

    /// <summary>
    /// Получаем весь состав
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список состава детали fatherId</returns>
    public async Task<List<GetDetailChildDto>?> GetAllAsync(GetAllChildrenFilters filters) =>
        await new DetailChildRequests(_context, _dataMapper).GetAllChildrenAsync(filters);

    public void Dispose() => _context.Dispose();
}
