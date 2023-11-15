using DB.Model.UserInfo;

namespace ServiceLayer.IServicesRepository.IUserServices;

public interface IUserAuthService : IDisposable
{
    Task<User?> GetUser(string login, string password);
}
