using AutoMapper;
using BizLayer.Builders.GraphBuilders;
using BizLayer.Repositories.GraphR;
using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BizLayer.Events.Graphs;

public class DeleteGraphsFromGroupEvent : IEvent
{
    private DbContext _context;
    private ErrorsMapper _errorsMapper;
    public DeleteFromGraphsGroupType DeleteType { get; }
    public List<OperationGraph> GroupGraphs { get; }

    public DeleteGraphsFromGroupEvent(DeleteFromGraphsGroupType deleteType, List<OperationGraph> groupGraphs)
    {
        DeleteType = deleteType;
        GroupGraphs = groupGraphs;
    }

    public void Execute(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    { }

    public async Task ExecuteAsync(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        _context = context;
        _errorsMapper = errorsMapper;

        // в зависимости от типа удаления, выполняем опеределенный код
        switch (DeleteType)
        {
            case DeleteFromGraphsGroupType.Full:
                DeleteFull();
                break;
            case DeleteFromGraphsGroupType.ToNewGroup:
                await DeleteToNewGroupAsync();
                break;
            case DeleteFromGraphsGroupType.ToSingle:
                await ToSingleAsync();
                break;
            default:
                break;
        }
    }

    private async Task ToSingleAsync()
    {
        var priority = await OperationGraphRead.LastOrDefaultPriorityAsync(_context, new List<int>());

        GroupGraphs.ForEach(g => g.Priority = ++priority);
    }

    private void DeleteFull() =>
        _context.Set<OperationGraph>().RemoveRange(GroupGraphs);

    private async Task DeleteToNewGroupAsync()
    {
        // создаем новую группу
        var graphGroupBuilder = new OperationGraphGroupBuilder(GroupGraphs.First(), GroupGraphs.Skip(1).ToList());
        await graphGroupBuilder.GroupingAsync(_context, _errorsMapper);
        graphGroupBuilder.Build();

        var priority = await OperationGraphRead.LastOrDefaultPriorityAsync(_context, new List<int>()) + 1;

        GroupGraphs.ForEach(g => g.Priority = priority);
    }
}