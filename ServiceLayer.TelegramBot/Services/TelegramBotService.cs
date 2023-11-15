using AutoMapper;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.UserInfo;
using ServiceLayer.TelegramBot.Services.Interfaces;
using Shared.Dto.Users;

namespace ServiceLayer.TelegramBot.Services;

public class TelegramBotService : ErrorsMapper, ITelegramBotService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<User> _repository;
    
    public TelegramBotService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _repository = new(_context, dataMapper);
    }

    public async Task<bool> ProfessionNumberValidationAsync(string professionNumber) => await _repository.FindFirstAsync(filter: u => u.ProfessionNumber == professionNumber) != null;

    public async Task<bool> PasswordValidationAsync(AuthInfoDto dto) => await _repository.FindFirstAsync(filter: u => u.ProfessionNumber == dto.Login && u.Password == dto.Password) != null;

    public Task<string?> GetFFLAsync(string professionNumber) => _repository.FindFirstAsync(filter: u => u.ProfessionNumber == professionNumber, select: u => u.FFL);

    public void Dispose() => _context.Dispose();
}
