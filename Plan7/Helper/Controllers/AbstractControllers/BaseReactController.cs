using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Plan7.Helper.Controllers.AbstractControllers;

[EnableCors("React")]
[Authorize]
[Route("api/p7serv/[controller]/[action]")]
[ApiController]
public abstract class BaseReactController<TLogging> : ControllerBase
    where TLogging : class
{
    protected readonly ILogger<TLogging> _logger;

    public BaseReactController(ILogger<TLogging> logger) => _logger = logger;
}
