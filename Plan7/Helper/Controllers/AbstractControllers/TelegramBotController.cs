using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Plan7.Helper.Controllers.AbstractControllers;

[EnableCors("TelegramBot")]
[Route("api/p7serv/[controller]/[action]")]
[ApiController]
public class TelegramBotController<TLogging> : ControllerBase
where TLogging : class
{
    protected readonly ILogger<TLogging> _logger;

    public TelegramBotController(ILogger<TLogging> logger) => _logger = logger;
}
