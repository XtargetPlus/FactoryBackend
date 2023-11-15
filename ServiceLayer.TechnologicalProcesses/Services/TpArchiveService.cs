using AutoMapper;
using BizLayer.Repositories.SubdivisionR;
using BizLayer.Repositories.TechnologicalProcessR;
using BizLayer.Repositories.UserR;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.StatusInfo;
using DB.Model.SubdivisionInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.UserInfo;
using ServiceLayer.TechnologicalProcesses.Services.Developer;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using Shared.Dto.TechnologicalProcess;
using Shared.Static;

namespace ServiceLayer.TechnologicalProcesses.Services;

/// <summary>
/// Сервис для архива тех процессов
/// </summary>
public class TpArchiveService : ErrorsMapper, ITpArchiveService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcess> _repository;
    private readonly IMapper _dataMapper;

    public TpArchiveService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Выдать тех процесс
    /// </summary>
    /// <param name="dto">Информация для выдачи</param>
    /// <returns>Выданный тех процесс</returns>
    public async Task IssueAsync(ArchiveChangeTechProcessStatusDto dto, int userId)
    {
        TpDeveloperService developerService = new(_context, _dataMapper);
        // Вызываем базовый методы изменения статуса разработки
        await developerService.ChangeTpStatusAsync(dto, (int)dto.TechProcessStatusId, userId);

        if (developerService.HasErrors)
            AddErrors(string.Join("\n", developerService.Errors));
        if (developerService.HasWarnings)
            AddWarnings(developerService.Warnings);
    }
    
    /// <summary>
    /// Выдать дубликат тех процесса
    /// </summary>
    /// <param name="dto">Информация для выдачи</param>
    /// <returns>Выданный дубликат или null (ошибки и предупреждения)</returns>
    public async Task IssueDuplicateAsync(IssueTechProcessDuplicateDto dto)
    {
        var techProcess = await TechProcessSimpleRead.GetAsync(_repository, dto.TechProcessId, this);

        if (!await TechProcessSimpleRead.IsIssuedAsync(dto.TechProcessId, _context) 
            && await TechProcessSimpleRead.IsSubdivisionHaveTechProcess(dto.TechProcessId, dto.SubdivisionId, _context))
        {
            AddErrors("Невозможно выдать тех процесс, убедитесь, что тех процесс был выдан и что в выбранное подразделение не было выдано дубликата");
        }

        if (HasErrors)
            return;

        await TechProcessSimpleRead.IncludeStatusesAsync(_repository, techProcess!);

        var issued = DateTime.Now;

        techProcess!.TechnologicalProcessStatuses!.Add(
            new()
            {
                StatusId = (int)TechProcessStatuses.IssuedDuplicate,
                StatusDate = issued,
                SubdivisionId = dto.SubdivisionId,
                UserId = dto.UserId,
            });

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление выданного дубликата
    /// </summary>
    /// <param name="techProcessStatusId">Id статуса тех процесса для удаления</param>
    /// <returns></returns>
    public async Task DeleteIssuedDuplicateAsync(int techProcessStatusId)
    {
        await _context.RemoveWithValidationAndSaveAsync(new TechnologicalProcessStatus { Id = techProcessStatusId },this);
    }

    public void Dispose() => _context.Dispose();
}
