namespace Plan7.Helper;

/// <summary>
/// 
/// </summary>
public class AddJwtTokenMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public AddJwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Проверяем роль пользователя в cookie, если пользователь ее поменял ручками, меняем ее на правильную
    /// </summary>
    /// <param name="context">Контекст HTTP запроса</param>
    /// <returns>Передаем запрос дальше</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies[".AspNetCore.Application.Id"];
        if (!string.IsNullOrEmpty(token))
            context.Request.Headers.Add("Authorization", "Bearer " + token);

        await _next.Invoke(context);
    }
}