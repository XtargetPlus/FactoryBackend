using DatabaseLayer.Helper;
using Shared.Dto.Users;

namespace ServiceLayer.TelegramBot.Services.Interfaces;

public interface ITelegramBotService : IErrorsMapper, IDisposable
{
    Task<bool> ProfessionNumberValidationAsync(string professionNumber);
    Task<bool> PasswordValidationAsync(AuthInfoDto dto);
    Task<string?> GetFFLAsync(string professionNumber);
}
