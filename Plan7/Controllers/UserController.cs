using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using Plan7.Helper.Controllers.Roles;
using ServiceLayer.IServicesRepository.IUserServices;
using Shared.Dto.Users;
using Shared.Dto.Users.Filters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Shared.Dto.Role;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер пользователя
/// </summary>
public class UserController : BaseReactController<UserController>
{
    private readonly IUserService _service;
    private readonly IUserCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public UserController(ILogger<UserController> logger, IUserService service, IUserCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление пользователя
    /// </summary>
    /// <param name="dto">Информация о пользователе</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = UserControllerRoles.Add)]
    public async Task<IActionResult> Add(BaseUserDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление пользователя - {ProfessionNumber}", HttpContext.User.FindFirstValue("UserId"), dto.ProfessionNumber);
        
        var id = await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors
            ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
            : Ok(id);
    }

    /// <summary>
    /// Добавление пользователю роли
    /// </summary>
    /// <param name="dto">Информация о новой роли и новом пароле пользователя</param>
    /// <returns>Ok или BadRequest или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = UserControllerRoles.ChangeRoleWithPassword)]
    public async Task<IActionResult> ChangeRoleWithPassword(ChangeUserRoleDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление роли: {setUserId} - {RoleId}",
            HttpContext.User.FindFirstValue("UserId"), dto.UserId, dto.RoleId);

        var newRole = await _service.ChangeRoleWithPasswordAsync(dto);
        if (_service.HasWarningsOrErrors)
            return BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings });

        // Если меняется роль текущего пользователя, переавторизируем его
        if (HttpContext.User.Identity is not ClaimsIdentity claimsIdentity) 
            return NotFound();
        
        if (int.TryParse(claimsIdentity.FindFirst("UserId").Value, out var userId) && userId != dto.UserId)
            return Ok();
        
        if (claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(ClaimsIdentity.DefaultRoleClaimType)))
        {
            claimsIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, newRole));
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            HttpContext.Response.Cookies.Append("role", newRole);

            await HttpContext.SignInAsync(claimsPrincipal);
        }

        return Ok();
    }

    /// <summary>
    /// Изменяем информацию о пользователе
    /// </summary>
    /// <param name="dto">Информация о пользователе</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = UserControllerRoles.Change)]
    public async Task<IActionResult> Change(UserChangeDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение пользователя: {dto}", HttpContext.User.FindFirstValue("UserId"), dto);

        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаляем пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление пользователя {deleteUserId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получаем список пользователей
    /// </summary>
    /// <param name="filters"></param>
    /// <returns>Список пользователей или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUserFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка пользователей", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync(filters.Text, filters.StatusId, filters.SearchOptions);

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получаем список пользователей по профессии
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetAllFromProfession)]
    public async Task<IActionResult> GetAllFromProfession([FromQuery] GetAllUserFromProfessionFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка пользователей по профессии{professionId}", HttpContext.User.FindFirstValue("UserId"), filters.ProfessionId);
        
        var count = await _count.GetFromProfessionAsync(filters.ProfessionId);

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllFromProfessionAsync(filters), Length = count });
    }

    /// <summary>
    /// Получаем список пользователей по подразделению
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetAllFromSubdivision)]
    public async Task<IActionResult> GetAllFromSubdivision([FromQuery] GetAllUserFromSubdivisionFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка пользователей по подразделениям {subdivisionId}", HttpContext.User.FindFirstValue("UserId"), filters.SubdivisionId);
        
        var count = await _count.GetFromSubdivAsync(filters.SubdivisionId);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllFromSubdivisionAsync(filters), Length = count });
    }

    /// <summary>
    /// Подробная информация о пользователе
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns>Получаем подробную информацию о пользователе вместе с полным списком подразделений, статусов, профессий</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetMoreInfo)]
    public async Task<ActionResult<UserListInfoDto>> GetMoreInfo([FromQuery, Range(1, int.MaxValue)] int id)
    {
        _logger.LogInformation("Пользователь {userId}: Получение информации о пользователе {existUserId} для редактирования", HttpContext.User.FindFirstValue("UserId"), id);
        
        var userListInfo = await _service.GetMoreInfoAsync(id);
        
        return _service.HasErrors ? Ok(new { Error = string.Join("\n", _service.Errors) }) : Ok(userListInfo);
    }

    /// <summary>
    /// Получаем подробную информацию для добавления
    /// </summary>
    /// <returns>Список подразделений, статусов, профессий</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetAddInfo)]
    public async Task<ActionResult<UserAddInfoDto>> GetAddInfo()
    {
        _logger.LogInformation("Пользователь {userId}: Получение информации для добавления пользователя", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAddInfoDtoAsync());
    }

    /// <summary>
    /// Получение списка технологов
    /// </summary>
    /// <returns>Список технологов</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetAllTechnologistsDevelopers)]
    public async Task<ActionResult<List<BaseUserGetDto>>> GetAllTechnologistsDevelopers()
    {
        _logger.LogInformation("Пользователь {userId}: Получение всех технологов", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetAllTechnologistsDevelopersAsync());
    }

    /// <summary>
    /// Получение роли пользователя
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <returns>Роль пользователя</returns>
    [HttpGet]
    [Authorize(Roles = UserControllerRoles.GetUserRole)]
    public async Task<ActionResult<UserRoleDto>> GetUserRole([FromQuery] int userId)
    {
        _logger.LogInformation("Пользователь {userId}: Получение роли пользователя", HttpContext.User.FindFirstValue("UserId"));
        
        return Ok(await _service.GetUserRoleAsync(userId));
    }

    /// <summary>
    /// Получение собственного id
    /// </summary>
    /// <returns>Id пользователя</returns>
    [HttpGet]
    [Authorize]
    public ActionResult<int> GetUserId()
    {
        _logger.LogInformation("Пользователь {userId}: Получение собственного id", HttpContext.User.FindFirstValue("UserId"));

        return Ok(HttpContext.User.FindFirstValue("UserId"));
    }
}
