using AutoMapper;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.StorageInfo.Graph;
using DB;
using ServiceLayer.Graphs.Services.Interfaces;
using Shared.Dto.Graph.Status;
using BizLayer.Repositories.GraphR;
using BizLayer.Repositories.GraphR.GraphStatusR;
using DatabaseLayer.DatabaseChange;
using Shared.Enums;

namespace ServiceLayer.Graphs.Services;

public class OperationGraphStatusService : ErrorsMapper, IOperationGraphStatusService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<OperationGraph> _repository;
    private readonly IMapper _dataMapper;

    public OperationGraphStatusService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }
    
    /// <summary>
    /// Смена статуса операционного графика
    /// </summary>
    /// <param name="dto">Информация для смены статуса</param>
    /// <returns></returns>
    public async Task ChangeAsync(OperationGraphChangeStatusDto dto)
    {
        var mainGraph = await OperationGraphRead.ByIdAsync(_context, dto.GraphId, this);
        await OperationGraphRead.LoadGroupGraphsAsync(_context, mainGraph, this);
        if (HasErrors) return;

        var statuses = OperationGraphStatusRead.PossibleStatuses((GraphStatus)mainGraph!.StatusId);
        if (statuses.All(s => s.StatusId != dto.StatusId))
        {
            AddErrors("Попытка смены статуса графика на недоступный");
            return;
        }

        mainGraph.StatusId = dto.StatusId;
        mainGraph.OperationGraphMainGroups!.ForEach(mg => mg.OperationGraphNext!.StatusId = dto.StatusId);

        if ((GraphStatus)mainGraph.StatusId is GraphStatus.Completed or GraphStatus.Canceled)
        {
            mainGraph.Priority = 0;
            mainGraph.ConfigrmingId = null;
            mainGraph.OperationGraphMainGroups!.ForEach(mg =>
            {
                mg.OperationGraphNext!.Priority = 0;
                mg.OperationGraphNext.ConfigrmingId = null;
            });
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    public void Dispose() => _context.Dispose();
}