using DatabaseLayer.Helper;
using Shared.Dto.Graph.Status;

namespace ServiceLayer.Graphs.Services.Interfaces;

public interface IOperationGraphStatusService : IErrorsMapper, IDisposable
{
    Task ChangeAsync(OperationGraphChangeStatusDto dto);
}