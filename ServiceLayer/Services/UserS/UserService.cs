using DB;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.IDbRequests.UserToDb;
using DatabaseLayer.IDbRequests;
using ServiceLayer.Services.SubdivisionS;
using ServiceLayer.IServicesRepository.IUserServices;
using ServiceLayer.Services.ProfessionS;
using Shared.Dto.Users;
using Shared.Dto.Users.Filters;
using ServiceLayer.Services.StatusS;
using BizLayer.Repositories.UserR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;
using Shared.Dto.Role;

namespace ServiceLayer.Services.UserR;

/// <summary>
/// Сервис пользователей
/// </summary>
public class UserService : ErrorsMapper, IUserService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<User> _repository;
    private readonly IMapper _dataMapper;

    public UserService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Асинхронное добавление пользователя
    /// </summary>
    /// <param name="dto">Информация для добавления пользователя</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями или null (ошибки и/или предупреждения)</returns>
    public async Task<int?> AddAsync(BaseUserDto dto)
    {
        User? user = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FathersName = dto.FathersName,
            FFL = $"{dto.LastName} {dto.FirstName[0]}.{dto.FathersName[0]}.",
            ProfessionNumber = dto.ProfessionNumber,
            Password = dto.Password,
            ProfessionId = dto.ProfessionId,
            SubdivisionId = dto.SubdivisionId,
            StatusId = dto.StatusId
        };
        user = await _context.AddWithValidationsAndSaveAsync(user, this);
        return user?.Id;
    }

    /// <summary>
    /// Меняем роль с паролем
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Наименование измененной роли или ошибки с предупреждениями или null (ошибки и/или предупреждения)</returns>
    public async Task<string?> ChangeRoleWithPasswordAsync(ChangeUserRoleDto dto)
    {
        var user = await _repository.FindFirstAsync(filter: u => u.Id == dto.UserId, i => i.Include(u => u.Role));
        if (user is null)
        {
            AddErrors("Не удалось получить пользователя");
            return null;
        }

        user.Password = dto.Password;
        if (user.RoleId is null || user.RoleId != dto.RoleId)
        {
            user.RoleId = dto.RoleId;
        }

        await _context.SaveChangesWithValidationsAsync(this);
        return await _repository.FindFirstAsync(u => u.Role!.Title, u => u.Id == dto.UserId);
    }

    /// <summary>
    /// Изменение информации о пользователе
    /// </summary>
    /// <param name="dto">Информация о пользователе, которую нужно изменить</param>
    /// <returns>1 или ошибки с предупреждениями или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(UserChangeDto dto)
    {
        var user = await UserReadWithValidations.GetAsync(_repository, dto.Id, this);
        if (user is null)
            return;

        if (user.FFL != $"{dto.LastName} {dto.FirstName[0]}.{dto.FathersName[0]}.") 
            user.FFL = $"{dto.LastName} {dto.FirstName[0]}.{dto.FathersName[0]}.";
        if (dto.FirstName is not null && user.FirstName != dto.FirstName) 
            user.FirstName = dto.FirstName;
        if (dto.LastName is not null && user.LastName != dto.LastName) 
            user.LastName = dto.LastName;
        if (dto.FathersName is not null && user.FathersName != dto.FathersName) 
            user.FathersName = dto.FathersName;
        if (dto.ProfessionNumber is not null && user.ProfessionNumber != dto.ProfessionNumber) 
            user.ProfessionNumber = dto.ProfessionNumber;
        if (dto.SubdivisionId is not 0 && user.SubdivisionId != dto.SubdivisionId)
            user.SubdivisionId = dto.SubdivisionId;
        if (dto.ProfessionId is not 0 && user.ProfessionId != dto.ProfessionId)
            user.ProfessionId = dto.ProfessionId;
        if (dto.StatusId is not 0 && user.StatusId != dto.StatusId)
            user.StatusId = dto.StatusId;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>1 или ошибки или null (ошибки)</returns>
    public async Task DeleteAsync(int id)
    {
        _repository.Remove(new User { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем пользователя с его информацией
    /// </summary>
    /// <param name="id">Id пользователя, которого нужно вытащить</param>
    /// <returns>Пользователь или null (ошибки)</returns>
    public async Task<UserGetForHubDto?> GetFirstAsync(int id)
    {
        var user = await _repository.FindFirstAsync<UserGetForHubDto>(filter: u => u.Id == id);

        if (user is null)
            AddErrors("Не удалось получить пользователя");
        return user;
    }

    /// <summary>
    /// Получаем список пользователей с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей</returns>
    public async Task<List<UserGetDto>?> GetAllAsync(GetAllUserFilters filters) =>
        await new UserRequests(_context, _dataMapper).GetAll(filters);

    /// <summary>
    /// Получаем список пользователей определенной профессии
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей</returns>
    public async Task<List<UserGetWithSubdivisionDto>?> GetAllFromProfessionAsync(GetAllUserFromProfessionFilters filters) =>
        await new UserRequests(_context, _dataMapper).GetProfAll(filters);

    /// <summary>
    /// Получаем список пользователей по подразделению
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей</returns>
    public async Task<List<UserGetProfessionDto>?> GetAllFromSubdivisionAsync(GetAllUserFromSubdivisionFilters filters) =>
        await new UserRequests(_context, _dataMapper).GetSubdivAll(filters);

    /// <summary>
    /// Получаем подробную информацию о пользователе
    /// </summary>
    /// <param name="id">Id пользователя, чью информацию нужно получить</param>
    /// <returns>Информация о пользователе, а так же список подразделений, профессий и статусов пользователей или null (ошибки)</returns>
    public async Task<UserListInfoDto?> GetMoreInfoAsync(int id)
    {
        UserListInfoDto userListInfo = new()
        {
            User = await _repository.FindFirstAsync<UserDto>(filter: w => w.Id == id)
        };
        if (userListInfo.User is null)
        {
            AddErrors("Не удалось получить пользователя");
            return null;
        }

        userListInfo.Info = await GetAddInfoDtoAsync();

        if (userListInfo.Info is null)
            AddErrors("Не удалось получить информацию для пользователей");
        if (userListInfo.Info?.Subdivisions is null)
            AddErrors("Не удалось получить список подразделений");
        if (userListInfo.Info?.Statuses is null)
            AddErrors("Не удалось получить список статусов пользователей");
        if (userListInfo.Info?.Professions is null)
            AddErrors("Не удалось получить список профессий");

        return HasErrors ? null : userListInfo;
    }

    /// <summary>
    /// Получаем информацию для добавления
    /// </summary>
    /// <returns>Список подразделений, профессий и статусов</returns>
    public async Task<UserAddInfoDto> GetAddInfoDtoAsync() =>
        new()
        {
            Subdivisions = await new SubdivisionService(_context, _dataMapper).GetAllAsync(),
            Statuses = await new StatusService(_context, _dataMapper).GetTableStatusesAsync("users"),
            Professions = await new ProfessionService(_context, _dataMapper).GetAllAsync(new() { Text = "", Skip = 0, Take = 0})
        };

    /// <summary>
    /// Получаем всех технологов разработчиков
    /// </summary>
    /// <returns>Список технологов разработчиков</returns>
    public async Task<List<BaseUserGetDto>?> GetAllTechnologistsDevelopersAsync() =>
        await _repository.GetAllAsync<BaseUserGetDto>(filter: u => u.RoleId == 7);

    /// <summary>
    /// Получаем роль пользователя
    /// </summary>
    /// <param name="userId">Id пользователя, чью роль мы хотим получить</param>
    /// <returns>Роль пользователя</returns>
    public async Task<UserRoleDto?> GetUserRoleAsync(int userId) =>
        await _repository.FindFirstAsync(
            select: u => u.Role == null ? null : new UserRoleDto() { Id = (int)u.RoleId!, Title = u.Role.Title },
            filter: u => u.Id == userId);

    public void Dispose() => _context.Dispose();
}
