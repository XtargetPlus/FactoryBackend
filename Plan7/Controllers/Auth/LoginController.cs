using Microsoft.AspNetCore.Mvc;
using ServiceLayer.IServicesRepository.IUserServices;
using Shared.Dto.Users;
using Microsoft.AspNetCore.Cors;
using Plan7.Helper;

namespace Plan7.Controllers.Auth;

/// <summary>
/// Авторизация
/// </summary>
[EnableCors("React")]
[Route("/api/p7serv/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserAuthService _service;
    private readonly ILogger<LoginController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public LoginController(ILogger<LoginController> logger, IUserAuthService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Login()
    {
        Response.ContentType = "text/html; charset=utf-8";
        Response.StatusCode = 200;
        return Ok();
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="dto">Данные для авторизации</param>
    /// <returns>Ok или что пользователь не авторизован</returns>
    [HttpPost]
    public async Task<IActionResult> Login(AuthInfoDto dto, IConfiguration configuration)
    {
        _logger.LogInformation("Проверка авторизации");
        var result = await _service.GetUser(dto.Login, dto.Password);
        if (result == null)
            return Unauthorized();

        var jwtAuthenticationManager = new JwtAuthenticationManager(configuration);
        var token = jwtAuthenticationManager.GenerateJwtToken(result);

        Response.Cookies.Append(".AspNetCore.Application.Id", token, new CookieOptions { MaxAge = TimeSpan.MaxValue });

        var response = new { Role = result.Role!.Title, Token = token };

        _logger.LogInformation("Пользователь авторизовался {userId}", result.Id);
        return Ok(response);
    }
}
