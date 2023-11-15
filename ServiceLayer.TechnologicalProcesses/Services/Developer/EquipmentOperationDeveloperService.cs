using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Enums;
using Shared.BasicStructuresExtensions;
using BizLayer.Repositories.TechnologicalProcessR.EquipmentOperationR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;
using Shared.Dto.TechnologicalProcess.EquipmentOperation.CUD;
using AutoMapper;

namespace ServiceLayer.TechnologicalProcesses.Services.Developer;

/// <summary>
/// Сервис операций на станках для разработчика тех процесса
/// </summary>
public class EquipmentOperationDeveloperService : ErrorsMapper, IEquipmentOperationDeveloperService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<EquipmentOperation> _repository;

    public EquipmentOperationDeveloperService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление операции на станке операции технического процесса
    /// </summary>
    /// <param name="dto">Информация на добавления</param>
    /// <returns></returns>
    public async Task AddAsync(EquipmentOperationDto dto)
    {
        EquipmentOperation? equipmentOperation = new()
        {
            EquipmentId = dto.EquipmentId,
            TechnologicalProcessItemId = dto.TechProcessItemId,
            DebugTime = dto.DebugTime,
            LeadTime = dto.LeadTime,
            Note = dto.Note,
            Priority = (byte)(_repository
                    .GetAll(eo => eo.TechnologicalProcessItemId == dto.TechProcessItemId)?
                    .OrderBy(eo => eo.Priority)
                    .LastOrDefault()?.Priority + 1 ?? 1)
        };
         await _context.AddWithValidationsAndSaveAsync(equipmentOperation, this);
    }

    /// <summary>
    /// Изменение операции на станке операции технического процесса
    /// </summary>
    /// <param name="dto">Информация на изменение операции на станке</param>
    /// <returns></returns>
    public async Task ChangeAsync(ChangeEquipmentOperationDto dto)
    {
        var equipmentOperation = await EquipmentOperationSimpleRead.GetByIdAsync(_repository, dto.EquipmentOperationId, this);
        if (HasErrors)
            return;
        
        if (dto.NewEquipmentId != 0 && equipmentOperation!.EquipmentId != dto.NewEquipmentId)
            equipmentOperation.EquipmentId = dto.NewEquipmentId;
        if (dto.Note != null && equipmentOperation!.Note != dto.Note)
            equipmentOperation.Note = dto.Note;
        if (equipmentOperation!.DebugTime != dto.DebugTime)
            equipmentOperation.DebugTime = dto.DebugTime;
        if (equipmentOperation.LeadTime != dto.LeadTime)
            equipmentOperation.LeadTime = dto.LeadTime;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Меняем местами операции на станке операции технического процесса
    /// </summary>
    /// <param name="dto">Информация для редактирования</param>
    /// <returns></returns>
    public async Task SwapAsync(SwapEquipmentOperationDto dto)
    {
        var firstEquipmentOperation = await EquipmentOperationSimpleRead.GetAsync(_repository, dto.FirstEquipmentId, dto.TechProcessItemId, this);
        var secondEquipmentOperation = await EquipmentOperationSimpleRead.GetAsync(_repository, dto.SecondEquipmentId, dto.TechProcessItemId, this);
        if (HasErrors)
            return;

        (firstEquipmentOperation!.Priority, secondEquipmentOperation!.Priority) = (secondEquipmentOperation.Priority, firstEquipmentOperation.Priority);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление операции на станке
    /// </summary>
    /// <param name="equipmentOperationId">Id операции тех процесса на станке</param>
    /// <returns></returns>
    public async Task DeleteAsync(int equipmentOperationId)
    {
        var equipmentOperation = await EquipmentOperationSimpleRead.GetByIdAsync(_repository, equipmentOperationId, this);
        if (HasErrors)
            return;

        var changedEquipmentOperations = await _repository.GetAllAsync(
            filter: eo => eo.TechnologicalProcessItemId == equipmentOperation!.TechnologicalProcessItemId && eo.Priority > equipmentOperation.Priority,
            trackingOptions: TrackingOptions.WithTracking);
        if (changedEquipmentOperations is null)
        {
            AddErrors("Не удалось найти операции на станках операции тех процесса");
            return;
        }

        changedEquipmentOperations.ForEach(eo => eo.Priority--);

        _repository.Remove(equipmentOperation);
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка операции на станке между двумя другими с перерасчетом приоритетов
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenAsync(InsertBetweenEquipmentOperation dto)
    {
        if (dto.BeforePriority == dto.CurrentTargetPriority)
        {
            AddErrors("Операция на станке №1 и операция на станке №2 одинаковы");
            return;
        }

        var isBeforePriorityNotZero = dto.BeforePriority != 0;

        var secondOperation = await EquipmentOperationSimpleRead.GetByPriorityAsync(_repository, dto.CurrentTargetPriority, dto.TechProcessItemId, this);
        if (HasErrors)
            return;

        var oldPriority = dto.CurrentTargetPriority;
        var newPriority = isBeforePriorityNotZero ? (dto.BeforePriority < oldPriority ? dto.BeforePriority + 1 : dto.BeforePriority) : 1;

        var operations = await _repository.GetAllAsync(
                filter: eo => eo.TechnologicalProcessItemId == dto.TechProcessItemId
                        && eo.EquipmentId != secondOperation!.EquipmentId
                        && ((oldPriority > newPriority) ? (eo.Priority >= newPriority && eo.Priority < oldPriority) : (eo.Priority <= newPriority && eo.Priority > oldPriority)),
                trackingOptions: TrackingOptions.WithTracking);

        if (operations.IsNullOrEmpty())
        {
            AddErrors("Не удалось получить список операций на станках");
            return;
        }

        operations!.ForEach(oe => oe.Priority += (byte)(oldPriority > newPriority ? 1 : -1));
        secondOperation!.Priority = (byte)newPriority;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public void Dispose() => _context.Dispose();
}
