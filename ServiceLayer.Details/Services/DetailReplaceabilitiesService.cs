using DatabaseLayer.IDbRequests;
using DB.Model.DetailInfo;
using DB;
using DatabaseLayer.IDbRequests.DetailToDb;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Detail;
using Shared.Dto.Detail.DetailChild;
using Shared.Dto.Detail.Filters;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.Details.Services.Interfaces;
using AutoMapper;

namespace ServiceLayer.Details.Services;

/*
 * Как работает заменяемость детали.
 * 
 * У нас есть детали [d1, d2, d3, d4, d5, d6]
 * 
 * Заменяемость детали - на бумаге детали разные, у них разный тех процесс, разное обозначение, но они могут заменять друг друга при сборке и тд.
 * Допустим у нас есть 2 подшибника - один подшибник производится по одному тех процессу, другой по другому, но они буквально одинаковы и тд.
 * 
 * Реализация заменяемости:
 * 
 * 1) Говорим, что d1 = d2 = d3 и d4 = d5 = d6
 * 2) Говорим, что d3 = d4
 *  2.1) Тогда d1 = d2 = d3 = d4 = d5 = d6
 * 3) Говорим, что d5 != d2 (удаляем d5 из заменяемости)
 *  3.1) Тогда d5 != d1 ; d5 != d2 ; d5 != d3 ; d5 != d4 ; d5 != d6
 */


/// <summary>
/// Сервис заменямости деталей
/// </summary>
public class DetailReplaceabilitiesService : ErrorsMapper, IDetailReplaceabilitiesService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<Detail> _repository;
    private readonly IMapper _dataMapper;

    public DetailReplaceabilitiesService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Добавление заменяемости
    /// </summary>
    /// <param name="id">Id детали, к которой добавляется заменяемости</param>
    /// <param name="serialNumber">Серийный номер другой детали, которую хотим добавить в заменяемость</param>
    /// <returns>Список новых заменяемостей детали с Id = id</returns>
    public async Task<List<BaseIdSerialTitleDto>?> AddAsync(TwoDetailIdDto dto)
    {
        var firstDetail = await _repository.FindFirstAsync(filter: d => d.Id == dto.FatherDetailId, include: i => i.Include(d => d.Ins));
        if (firstDetail is null)
            AddErrors("Не удалось получить первую деталь");
        var secondDetail = await _repository.FindFirstAsync(filter: d => d.Id == dto.ChildDetailId, include: i => i.Include(d => d.Ins));
        if (secondDetail is null)
            AddErrors("Не удалось получить вторую деталь");
        if (HasErrors)
            return null;

        /* 
         * Создаем новые заменяемости для первой и второй детали 
         * Вытягиваем список заменяемых деталей из первой детали и добавляем к списку заменимости второй детали
         */
        var newFirstDetailItems = await CreateNewReplaceabilitiesListAsync(firstDetail!, secondDetail!);
        var newSecondDetailItems = await CreateNewReplaceabilitiesListAsync(secondDetail!, firstDetail!);

        if (newFirstDetailItems is null || newSecondDetailItems is null)
            return null;

        // В результирующий список новой заменяемости детали firstDetail добавляем деталь secondDetail и затем все ее заменяемости
        List<BaseIdSerialTitleDto> results = new() { new() { DetailId = secondDetail!.Id, SerialNumber = secondDetail.SerialNumber, Title = secondDetail.Title } };
        newSecondDetailItems.ForEach(d => results.Add(new() { DetailId = d.SecondDetail.Id, SerialNumber = d.SecondDetail.SerialNumber, Title = d.SecondDetail.Title }));
        
        newFirstDetailItems.ForEach(d => secondDetail.Ins!.Add(d));
        newSecondDetailItems.ForEach(d => firstDetail!.Ins!.Add(d));

        // выполняем d1 = d2
        firstDetail!.Ins!.Add(new DetailReplaceability { FirstDetail = firstDetail, SecondDetail = secondDetail! });
        secondDetail!.Ins!.Add(new DetailReplaceability { FirstDetail = secondDetail, SecondDetail = firstDetail });

        /* 
         * Выполняем d1 = d2 = d3 = d4 = dn
         *
         *
         * Рисуем пентаграмму, когда все детали связаны друг с другом
         */
        foreach (var targetDetail in secondDetail.Ins)
        {
            foreach (var addedDetail in secondDetail.Ins.Where(addedDetail => targetDetail.SecondDetailId != addedDetail.SecondDetailId
                                                                              && targetDetail.SecondDetail.Ins!.All(d => d.SecondDetail.Id != addedDetail.SecondDetail.Id)))
            {
                targetDetail.SecondDetail.Ins!.Add(new() { FirstDetail = targetDetail.SecondDetail, SecondDetail = addedDetail.SecondDetail });
            }
        }

        await _context.SaveChangesWithValidationsAsync(this);
        return results;
    }

    /// <summary>
    /// Удаление заменямости у детали
    /// </summary>
    /// <param name="detailId">Id удаляемой детали из заменямости</param>
    /// <returns></returns>
    public async Task DeleteAsync(int detailId)
    {
        var detail = await _repository.FindFirstAsync(filter: d => d.Id == detailId, include: i => i.Include(d => d.Ins).Include(d => d.Outs));
        if (detail is null)
        {
            AddErrors("Не удалось получить деталь");
            return;
        }

        detail.Ins!.Clear();
        detail.Outs!.Clear();

        _repository.Update(detail);
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Получаем первую заменямость
    /// </summary>
    /// <param name="detailId">Id второй детали</param>
    /// <returns>Деталь или null (ошибки)</returns>
    public async Task<BaseIdSerialTitleDto?> GetFirstAsync(int detailId)
    {
        var detailDto = await _repository.FindFirstAsync<BaseIdSerialTitleDto>(
            filter: d => d.Ins.FirstOrDefault(di => di.SecondDetailId == detailId) != null);
        if (detailDto is null)
            AddErrors("Не удалось получить деталь из заменяемости");
        return detailDto;
    }

    /// <summary>
    /// Получаем весь список заменяемости детали
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список заменяемости</returns>
    public async Task<List<BaseIdSerialTitleDto>?> GetAllAsync(GetAllReplaceabilityFilters filters) =>
        await new DetailRequests(_context, _dataMapper).GetAllReplaceabilitiesAsync(filters);

    /// <summary>
    /// Создаем новую заменямости для детали secondDetail
    /// </summary>
    /// <param name="firstDetail">Деталь, из которой мы вытягиваем ее заменяемость</param>
    /// <param name="secondDetail">Деталь, для к которой мы будем добавлять новую заменяемость</param>
    /// <returns>Список новой заменямости для детали secondDetail, которую мы получили из firstDetail или null (ошибки)</returns>
    private async Task<List<DetailReplaceability>?> CreateNewReplaceabilitiesListAsync(Detail firstDetail, Detail secondDetail)
    {
        List<DetailReplaceability> newReplaceabilitiesList = new();
        foreach (var detailR in firstDetail!.Ins!)
        {
            var detail = await _repository.FindFirstAsync(filter: d => d.Id == detailR.SecondDetailId, include: i => i.Include(d => d.Ins));
            if (detail is null)
            {
                AddErrors("Не удалось получить заменяемую деталь");
                return null;
            }
            detail.Ins!.Add(new DetailReplaceability { FirstDetail = detail, SecondDetail = secondDetail! });
            newReplaceabilitiesList.Add(new DetailReplaceability { FirstDetail = secondDetail!, SecondDetail = detail });
        }
        return newReplaceabilitiesList;
    }

    public void Dispose() => _context.Dispose();
}
