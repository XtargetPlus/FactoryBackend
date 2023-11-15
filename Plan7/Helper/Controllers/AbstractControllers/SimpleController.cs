using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Plan7.Helper.Controllers.AbstractControllers;

public abstract class SimpleController<T, TLogging> : BaseReactController<TLogging>
    where T : class
    where TLogging : class
{
    protected SimpleController(ILogger<TLogging> logger) 
        : base(logger) { }

    public abstract Task<IActionResult> Add(TitleDto dto);
    public abstract Task<IActionResult> Change(T obj);
    public abstract Task<IActionResult> Delete(int id);
    public abstract Task<IActionResult> GetAll([FromQuery, MaybeNull] string text);
}
