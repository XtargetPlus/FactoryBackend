using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.UserInfo;

namespace BizLayer.Repositories.UserR;

public static class UserReadWithValidations
{
    public static async Task<User?> GetAsync(BaseModelRequests<User> repository, int userId, ErrorsMapper mapper)
    {
        User? user = await repository.FindByIdAsync(userId);
        if (user is null)
            mapper.AddErrors("Не удалось получить сотрудника");
        return user;
    }
}