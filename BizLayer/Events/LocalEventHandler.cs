using AutoMapper;
using DatabaseLayer.Helper;
using Microsoft.EntityFrameworkCore;

namespace BizLayer.Events;

public class LocalEventHandler : ILocalEventHandler
{
    private readonly IEvent _event;

    public LocalEventHandler(IEvent classEvent)
    {
        _event = classEvent;
    }

    public void Execute(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        _event.Execute(context, dataMapper, errorsMapper);
    }

    public async Task ExecuteAsync(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper)
    {
        await _event.ExecuteAsync(context, dataMapper, errorsMapper);
    }
}