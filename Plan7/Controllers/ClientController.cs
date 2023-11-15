using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;
using ServiceLayer.IServicesRepository.IClientServices;
using Plan7.Helper.Controllers.Roles;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер заказчика
/// </summary>
public class ClientController : BaseReactController<ClientController>
{
	private readonly IClientService _service;
	private readonly IClientCountService _count;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="clientService"></param>
	/// <param name="count"></param>
	public ClientController(ILogger<ClientController> logger, IClientService clientService, IClientCountService count)
		: base(logger)
	{ 
		_service = clientService;
		_count = count;
	}

	/// <summary>
	/// Добавление
	/// </summary>
	/// <param name="dto">Наименование</param>
	/// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
	[HttpPost]
	[Authorize(Roles = ClientControllerRoles.Add)]
	public async Task<IActionResult> Add(TitleDto dto)
	{
		_logger.LogInformation("Пользователь {userId}: Добавление клиента: {Title}", HttpContext.User.FindFirstValue("UserId"), dto.Title);

		var id = await _service.AddAsync(dto);
		
		return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
	}

	/// <summary>
	/// Изменение 
	/// </summary>
	/// <param name="dto">Информация об заказчике</param>
	/// <returns>Ok или ошибки с предупреждениями</returns>
	[HttpPost]
	[Authorize(Roles = ClientControllerRoles.Change)]
	public async Task<IActionResult> Change(BaseDto dto)
	{
		_logger.LogInformation("Пользователь {userId}: Изменение клиента: {clientId} - {Title}", HttpContext.User.FindFirstValue("UserId"),dto.Id, dto.Title);

		await _service.ChangeAsync(dto);
		
		return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
	}

	/// <summary>
	/// Удаление
	/// </summary>
	/// <param name="id">Id удаляемой записи</param>
	/// <returns></returns>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = ClientControllerRoles.Delete)]
	public async Task<IActionResult> Delete(int id)
	{
		_logger.LogInformation("Пользователь {userId}: Удаление клиента: {clientId}", HttpContext.User.FindFirstValue("UserId"), id);

		await _service.DeleteAsync(id);

		return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
	}

	/// <summary>
	/// Получение списка заказчиков
	/// </summary>
	/// <param name="take">Сколько получить</param>
	/// <param name="skip">Сколько пропустить</param>
	/// <returns>Список заказчиков или Length = 0</returns>
	[HttpGet]
	[Authorize(Roles = ClientControllerRoles.GetAll)]
	public async Task<IActionResult> GetAll([FromQuery, DefaultValue(50), Range(0, int.MaxValue)] int take, [FromQuery, DefaultValue(0), Range(0, int.MaxValue)] int skip)
	{
		_logger.LogInformation("Пользователь {userId}: Получение списка клиентов", HttpContext.User.FindFirstValue("UserId"));

		var count = await _count.GetAllAsync();
		
		return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(take, skip), Length = count });
	}
}
