using DB;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IUserServices;

namespace ServiceLayer.Services.UserR;

/// <summary>
/// Сервис авторизации пользователя
/// </summary>
public class UserAuthService : IUserAuthService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<User> _repository;

    public UserAuthService(DbApplicationContext db)
    {
        _context = db;
        _repository = new(_context, null);
    }
    public UserAuthService() { }

    /// <summary>
    /// Получаем пользователя
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    /// <returns>Пользователь или null</returns>
    public async Task<User?> GetUser(string login, string password) =>
        await _repository.FindFirstAsync(
            filter: u => u.ProfessionNumber == login && u.Password == password,
            include: i => i.Include(u => u.Profession).Include(u => u.Role));

    public void Dispose() => _context.Dispose();  
}
