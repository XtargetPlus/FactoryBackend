using DatabaseLayer.Helper;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.Tools;

namespace ServiceLayer.Tools.Services.Interfaces;

public interface IToolParameterService : IErrorsMapper, IDisposable
{
    Task<int?> AddParameterAsync(AddToolParameterDto dto);
    Task ChangeParameterAsync(ChangeToolParameterDto dto);
    Task DeleteParameterAsync(int id);

    Task<List<GetToolParameterDto>?> GetParametersAsync();

}