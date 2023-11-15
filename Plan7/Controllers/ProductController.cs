using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.Roles;
using ServiceLayer.IServicesRepository.IProductServices;
using System.Security.Claims;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto.Product;
using Shared.Dto.Product.Filters;
using Shared.Dto.Detail;

namespace Plan7.Controllers;

/// <summary>
/// Контроллер изделия
/// </summary>
public class ProductController : BaseReactController<ProductController>
{
    private readonly IProductService _service;
    private readonly IProductCountService _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="count"></param>
    public ProductController(ILogger<ProductController> logger, IProductService service, IProductCountService count)
        : base(logger)
    {
        _service = service;
        _count = count;
    }

    /// <summary>
    /// Добавление изделия
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленного изделия или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = ProductControllerRoles.Add)]
    public async Task<IActionResult> Add(BaseProductDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление изделия: {productId} - {Price}", HttpContext.User.FindFirstValue("UserId"), dto.DetailId, dto.Price);

        var id = await _service.AddAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Изменение изделия
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = ProductControllerRoles.Change)]
    public async Task<IActionResult> Change(ProductDto dto)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение изделия:{productId} - {Price}", HttpContext.User.FindFirstValue("UserId"), dto.ProductId, dto.Price);

        await _service.ChangeAsync(dto);

        return _service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings }) : Ok();
    }

    /// <summary>
    /// Удаление изделия
    /// </summary>
    /// <param name="id">Id изделия</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = ProductControllerRoles.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление изделия: {productId}", HttpContext.User.FindFirstValue("UserId"), id);

        await _service.DeleteAsync(id);

        return _service.HasErrors ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка изделий
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список пользователей с количеством или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = ProductControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductFilters filters)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка изделий", HttpContext.User.FindFirstValue("UserId"));

        var count = await _count.GetAllAsync(filters.Text, filters.ProductSearch);

        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await _service.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение списка изделий без пагинации для селектов и тд
    /// </summary>
    /// <returns>Список изделий</returns>
    [HttpGet]
    [Authorize(Roles = ProductControllerRoles.GetAllForChoice)]
    public async Task<ActionResult<IEnumerable<BaseIdSerialTitleDto>>> GetAllForChoice()
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка изделий для выбора", HttpContext.User.FindFirstValue("UserId"));

        return Ok(await _service.GetAllForChoiceAsync());
    }
}
