using DatabaseLayer.IDbRequests;
using DB.Model.UserInfo.RoleInfo;
using DB;
using ServiceLayer.IServicesRepository;
using DB.Model.UserInfo;
using Shared.Dto.Role;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using AutoMapper;

namespace ServiceLayer.Services.RoleS;

/// <summary>
/// Сервис функционала роли на разных формах
/// </summary>
public class RoleClientService : ErrorsMapper, IRoleClientService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<RoleClient> _roleClientRepository;
    private readonly BaseModelRequests<Role> _roleRepository;

    public RoleClientService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _roleClientRepository = new(_context, dataMapper);
        _roleRepository = new(_context, dataMapper);
    }

    /// <summary>
    /// Добавление функционала роли на форме
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>Добавленная запись с функционалом или null (ошибки и/или предупреждения)</returns>
    public async Task AddValueAsync(RoleClientDto dto)
    {
        RoleClient roleClient = new()
        {
            RoleId = dto.RoleId,
            UserFormId = dto.UserFormId,
            Add = dto.Func.Add,
            Edit = dto.Func.Edit,
            Delete = dto.Func.Delete,
            Browsing = dto.Func.Browsing
        };
        await _context.AddWithValidationsAndSaveAsync(roleClient, this);
    }

    /// <summary>
    /// Множественное добавление функционала на формах для роли
    /// </summary>
    /// <param name="dto">Информация на добавление</param>
    /// <returns>1 или null (ошибки с предупреждениями)</returns>
    public async Task AddRangeAsync(RoleAddRangeWithFuncOnFormsDto dto)
    {
        var role = await _roleRepository.FindByIdAsync(dto.RoleId);
        if (role is null)
        {
            AddErrors("Не удалось получить роль");
            return;
        }

        role.RoleClients = new();
        foreach (var formFunc in dto.FuncInForms)
        {
            role.RoleClients.Add(new RoleClient
            {
                RoleId = dto.RoleId,
                UserFormId = formFunc.Key,
                Add = formFunc.Value.Add, 
                Edit = formFunc.Value.Edit,
                Delete = formFunc.Value.Delete,
                Browsing = true
            });
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Изменяем запись
    /// </summary>
    /// <param name="dto">Информация на изменение</param>
    /// <returns>1 или null (ошибки и/или предупреждения)</returns>
    public async Task ChangeAsync(RoleClientDto dto)
    {
        var roleClient = await _roleClientRepository.FindFirstAsync(filter: rc => rc.RoleId == dto.RoleId && rc.UserFormId == dto.UserFormId);
        if (roleClient is null)
        {
            AddErrors("Не удалось получить функционал");
            return;
        }

        roleClient.Add = dto.Func.Add;
        roleClient.Edit = dto.Func.Edit;
        roleClient.Browsing = dto.Func.Browsing;
        roleClient.Delete = dto.Func.Delete;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Множественное изменение функционала на формах для роли
    /// </summary>
    /// <param name="dto">Информация для множественного изменения</param>
    /// <returns>1 или null (ошибки с предупреждениями)</returns>
    public async Task ChangeRangeAsync(RoleChangeRangeWithFuncOnFormsDto dto)
    {
        var role = await _roleRepository.FindFirstAsync(filter: r => r.Id == dto.RoleId, include: i => i.Include(r => r.RoleClients));
        if (role is null)
        {
            AddErrors("Не удалось получить роль");
            return;
        }
        role.Title = dto.Title;
        foreach (var form in dto.FuncInForms)
        {
            var roleClient = role.RoleClients!.FirstOrDefault(rc => rc.UserFormId == form.Key);
            if (roleClient is not null)
            {
                roleClient.Delete = form.Value.Delete;
                roleClient.Add = form.Value.Add;
                roleClient.Edit = form.Value.Edit;
                roleClient.Browsing = form.Value.Browsing;
            }
            else
            {
                role.RoleClients!.Add(new()
                {
                    UserFormId = form.Key,
                    Delete = form.Value.Delete,
                    Add = form.Value.Add,
                    Edit = form.Value.Edit,
                    Browsing = form.Value.Browsing,
                });
            }
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление функционала роли на форме
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>1 или null (ошибка)</returns>
    public async Task DeleteAsync(BaseRoleClientDto dto)
    {
        _roleClientRepository.Remove(new RoleClient { UserFormId = dto.UserFormId, RoleId = dto.RoleId });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Множественное удаление функционала на формах для роли
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>1 или null (ошибки)</returns>
    public async Task DeleteRangeAsync(RoleClientDeleteRangeDto dto)
    {
        var roleClients = await _roleClientRepository.GetAllAsync(
            filter: rc => rc.RoleId == dto.RoleId && dto.FormsId.Contains(rc.UserFormId),
            trackingOptions: TrackingOptions.WithTracking);
        if (roleClients == null || roleClients.Count < 1)
        {
            AddErrors("Не удалось получить функционал роли");
            return;
        }

        _roleClientRepository.Remove(roleClients);
        await _context.SaveChangesWithValidationsAsync(this, false);
    }

    /// <summary>
    /// Получаем список функционалов ролей на формах
    /// </summary>
    /// <returns>Список функционалов ролей на формах</returns>
    public async Task<List<RoleClientConcreteDto>?> GetAllAsync() => await _roleClientRepository.GetAllAsync<RoleClientConcreteDto>();

    /// <summary>
    /// Получаем функционала роли на форме
    /// </summary>
    /// <param name="form">Форма</param>
    /// <param name="role">Роль</param>
    /// <returns>Функционал роли role на форме form</returns>
    public async Task<RoleClientFuncDto?> GetFormFuncAsync(string form, string role) =>
        await _roleClientRepository.FindFirstAsync<RoleClientFuncDto>(filter: rc => rc.UserForm!.Title == form && rc.Role!.Title == role);

    /// <summary>
    /// Фильтруем список форм, которые может видеть пользователь
    /// </summary>
    /// <param name="forms">Список неотфильтрованных форм</param>
    /// <param name="role">Роль пользователя</param>
    /// <returns>Возвращаем только те формы, которые может видеть пользователь</returns>
    public async Task<List<string>?> GetUserFormsAsync(List<string> forms, string role) =>
        (await _roleClientRepository.GetAllAsync(filter: rc => rc.Role!.Title == role, select: rc => rc.UserForm!.Title) ?? new()).Intersect(forms).ToList();

    public void Dispose() => _context.Dispose();  
}
