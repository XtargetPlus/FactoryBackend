using DB.Model.UserInfo;
using DB;
using DatabaseLayer.IDbRequests;
using ServiceLayer.IServicesRepository.IUserServices;
using Shared.Enums;

namespace ServiceLayer.Services.UserR;

/// <summary>
/// Сервис подсчета количества пользователей с условиями или без
/// </summary>
public class UserCountService : IUserCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<User> _count;

    public UserCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    public async Task<int?> GetAllAsync(int statusId) => statusId != 0 ? await _count.CountAsync(u => u.StatusId == statusId) : await GetAllAsync();

    // X = x if x > 5 else 0 

    /// <summary>
    /// Считаем количество пользователей с учетом их статуса
    /// </summary>
    /// <param name="statusId">Id статуса или 0 если нужно посчитать пользователей не учитывая статус</param>
    /// <returns>Количество пользователей</returns>
    public async Task<int?> GetAllAsync(string text, int statusId, UserSearchOptions searchOptions = default)
    {
        if (string.IsNullOrEmpty(text))
            return await GetAllAsync(statusId);

        return searchOptions switch
        {
            UserSearchOptions.ForFfl => await _count.CountAsync(u => statusId > 0
                                                                    ? u.FFL.Contains(text) && u.StatusId == statusId
                                                                    : statusId <= 0
                                                                        ? u.FFL.Contains(text)
                                                                        : u.StatusId == statusId),
            UserSearchOptions.ForProfNumber => await _count.CountAsync(u => statusId > 0
                                                                    ? u.ProfessionNumber.Contains(text) && u.StatusId == statusId
                                                                    : statusId <= 0
                                                                        ? u.ProfessionNumber.Contains(text)
                                                                        : u.StatusId == statusId),
            _ => await GetAllAsync(statusId)
        };
    }

    /// <summary>
    /// Количество пользователей с учетом их подразделения
    /// </summary>
    /// <param name="subdivisionId">Id подразделения</param>
    /// <returns>Количество пользователей</returns>
    public async Task<int?> GetFromSubdivAsync(int subdivisionId) => await _count.CountAsync(u => u.SubdivisionId == subdivisionId);

    /// <summary>
    /// Количество пользователей с учетом их профессии
    /// </summary>
    /// <param name="professionId">Id профессии</param>
    /// <returns>Количество пользователей</returns>
    public async Task<int?> GetFromProfessionAsync(int professionId) => await _count.CountAsync(u => u.ProfessionId == professionId);

    public void Dispose() => _context.Dispose();
}
