using DatabaseLayer.IDbRequests;
using DB.Model.TechnologicalProcessInfo;
using DB;
using Shared.Dto.TechnologicalProcess;
using Shared.Static;
using BizLayer.Repositories.TechnologicalProcessR;
using Shared.Enums;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;
using AutoMapper;

namespace ServiceLayer.TechnologicalProcesses.Services.Developer;

/// <summary>
/// Сервис для разработчиков технических процессов
/// </summary>
public class TpDeveloperService : ErrorsMapper, ITpDeveloperService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcess> _repository;

    public TpDeveloperService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Изменение подробной информации технического процесса
    /// </summary>
    /// <param name="dto">Информация на изменения</param>
    /// <returns></returns>
    public async Task ChangeTpDataInfoAsync(ChangeTechProcessData dto)
    {
        var techProcess = await TechProcessSimpleRead.GetWithIncludeDataAsync(_repository, dto.TechProcessId, this);
        if (HasErrors)
            return;

        if (techProcess!.TechnologicalProcessData.BlankTypeId != dto.BlankTypeId) techProcess.TechnologicalProcessData.BlankTypeId = dto.BlankTypeId;
        if (techProcess.TechnologicalProcessData.MaterialId != dto.MaterialId) techProcess.TechnologicalProcessData.MaterialId = dto.MaterialId;
        if (techProcess.TechnologicalProcessData.Rate != dto.Rate) techProcess.TechnologicalProcessData.Rate = dto.Rate;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменение статуса разработки тех процесса
    /// </summary>
    /// <param name="dto">Базовая информация для изменения</param>
    /// <param name="statusId">Id статуса, на который нужно поменять текущий статус</param>
    /// <returns></returns>
    public async Task ChangeTpStatusAsync(BaseChangeTechProcessStatusDto dto, int statusId, int userId)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, this);
        if (HasErrors)
            return;

        if ((TechProcessStatuses)statusId == TechProcessStatuses.Issued)
        {
            var techProcesses = await _repository.GetAllAsync(
                filter: tp => tp.Id != techProcess!.Id
                              && tp.DeveloperId == techProcess.DeveloperId
                              && tp.DevelopmentPriority > techProcess.DevelopmentPriority
                              && tp.TechnologicalProcessStatuses!.All(tps => tps.StatusId != (int)TechProcessStatuses.Issued),
                trackingOptions: TrackingOptions.WithTracking);

            techProcesses!.ForEach(tp => tp.DevelopmentPriority--);
            techProcess!.DevelopmentPriority = 0;
        }

        techProcess!.TechnologicalProcessStatuses = new()
        {
            new() 
            { 
                StatusId = statusId, 
                TechnologicalProcessId = techProcess.Id, 
                Note = dto.Note, 
                StatusDate = dto.Added,
                UserId = userId
            }
        };

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Смена актуальности (видимости) тех процесса
    /// </summary>
    /// <param name="dto">Информация для смены актуальности</param>
    /// <returns></returns>
    public async Task ChangeActualTProcessAsync(ChangeTechProcessActualDto dto)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, QueryFilterOptions.TurnOff, this);
        if (HasErrors) 
            return;

        techProcess!.IsActual = dto.IsActual;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public void Dispose() => _context.Dispose();
}
