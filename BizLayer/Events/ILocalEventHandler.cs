using AutoMapper;
using DatabaseLayer.Helper;
using Microsoft.EntityFrameworkCore;

namespace BizLayer.Events;

public interface ILocalEventHandler
{
    void Execute(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper);
    Task ExecuteAsync(DbContext context, IMapper dataMapper, ErrorsMapper errorsMapper);
}