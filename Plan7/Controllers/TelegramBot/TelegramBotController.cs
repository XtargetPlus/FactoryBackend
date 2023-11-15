using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using ServiceLayer.TelegramBot.Services.Interfaces;
using Shared.Dto.Users;
using System.Security.Claims;

namespace Plan7.Controllers.TelegramBot;

/// <summary>
/// Контроллер для обращения телеграм бота
/// </summary>
public class TelegramBotController : TelegramBotController<TelegramBotController>
{
    private readonly ITelegramBotService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TelegramBotController(ILogger<TelegramBotController> logger, ITelegramBotService service) : base(logger) => _service = service; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="professionNumber"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ProfessionNumberValidation([FromQuery] string professionNumber)
    {
        _logger.LogInformation("Пользователь {userId}: Валидация табельного номера {professionNumber} для авторизации в телеграмм боте", HttpContext.User.FindFirstValue("UserId"), professionNumber);

        return await _service.ProfessionNumberValidationAsync(professionNumber) ? Ok() : NotFound();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> PasswordValidation([FromQuery] AuthInfoDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Валидация пароля {password} пользователя {professionNumber} для авторизации в телеграмм боте", 
            HttpContext.User.FindFirstValue("UserId"), dto.Password, dto.Login);

        return await _service.PasswordValidationAsync(dto) ? Ok() : NotFound();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="professionNumber"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetFFL([FromQuery] string professionNumber)
    {
        _logger.LogInformation(
            "Пользователь {userId}: Получение ФИО пользователя с табельным номером {professionNumber}",
            HttpContext.User.FindFirstValue("UserId"), professionNumber);
        
        return Ok(await _service.GetFFLAsync(professionNumber));
    }

    /// <summary>
    /// Для тестирования
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendNews([FromBody] NewsDto dto)
    {
        HttpClient telegramBot = new() { BaseAddress = new Uri("https://localhost:5051/") };
        using var response = await telegramBot.PostAsJsonAsync($"/api/SendNews", new NewsDto(dto.Message, dto.Subscribe));
        
        return Ok();
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="Message"></param>
/// <param name="Subscribe"></param>
public record NewsDto(string Message, int Subscribe);
