using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Plan7.Controllers.Auth;

/// <summary>
/// Контроллер выхода
/// </summary>
[EnableCors("React")]
[Authorize]
[Route("/api/p7serv/logout")]
public class LogoutControllerTest : Controller
{
    /// <summary>
    /// Выход
    /// </summary>
    /// <returns>Редирект на главную страницу</returns>
    [HttpGet]
    public async Task<IActionResult> Logout()
    {   
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}
