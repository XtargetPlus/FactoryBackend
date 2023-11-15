using System.Security.Claims;

namespace Plan7.Helper;

/// <summary>
/// Проверка роли пользователя
/// </summary>
public class CheckRoleMiddleware
{
    private RequestDelegate next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public CheckRoleMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Проверяем роль пользователя в cookie, если пользователь ее поменял ручками, меняем ее на правильную
    /// </summary>
    /// <param name="context">Контекст HTTP запроса</param>
    /// <returns>Передаем запрос дальше</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User.Identity;
        if (user is not null && user.IsAuthenticated)
        {
            if (context.Request.Cookies.ContainsKey("role")
                && context.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) != null
                && context.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) is string role)
            {
                if (context.Request.Cookies["role"] != role)
                {
                    context.Response.Cookies.Delete("role");
                    context.Response.Cookies.Append("role", role);
                }
            }
        }
        await next.Invoke(context);
    }
}
