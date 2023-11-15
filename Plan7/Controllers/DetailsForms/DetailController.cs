using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Plan7.Helper.Controllers.Roles.DetailsForms;
using Plan7.Helper.Controllers.AbstractControllers;
using Shared.Dto.Detail;
using Shared.Dto.Detail.DetailChild;
using System.ComponentModel.DataAnnotations;
using Shared.Dto.Detail.DetailChild.Filters;
using Shared.Dto.Detail.Filters;
using ServiceLayer.Details.Services.Interfaces;

namespace Plan7.Controllers.DetailsForms;

/// <summary>
/// Контроллер детали
/// </summary>
public class DetailController : BaseReactController<DetailController>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public DetailController(ILogger<DetailController> logger) 
        : base(logger) { }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id добавленной записи или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.Add)]
    public async Task<IActionResult> Add(DetailMoreInfoDto dto, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление детали: {Title} - {SerialNumber}", HttpContext.User.FindFirstValue("UserId"), dto.Title, dto.SerialNumber);
        
        var id = await service.AddAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok(id);
    }

    /// <summary>
    /// Добавление детали в состав
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id детали, которую добавили, с единицей измерения или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.AddChild)]
    public async Task<IActionResult> AddChild(DetailChildAddDto dto, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Добавление состава детали: {FatherId} - {ChildId}", HttpContext.User.FindFirstValue("UserId"), dto.FatherId, dto.ChildId);
        
        await service.AddAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Добавление детали в заменяемость
    /// </summary>
    /// <param name="dto">Информация для добавления</param>
    /// <returns>Id детали, которую добавили или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.AddReplaceability)]
    public async Task<IActionResult> AddReplaceability(TwoDetailIdDto dto, IDetailReplaceabilitiesService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка заменяемости: {FatherDetailId} - {ChildDetailId}", HttpContext.User.FindFirstValue("UserId"), dto.FatherDetailId, dto.ChildDetailId);
        
        var details = await service.AddAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok(details);
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.Change)]
    public async Task<IActionResult> Change(DetailChangeDto dto, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение детали {DetailId}: {Title} - {SerialNumber}", HttpContext.User.FindFirstValue("UserId"), dto.Id, dto.Title, dto.SerialNumber);
       
        await service.ChangeAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Изменение записи в составе детали 
    /// </summary>
    /// <param name="dto">Информация для изменения</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.ChangeChild)]
    public async Task<IActionResult> ChangeChild(DetailChildAddDto dto, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Изменение состава детали: {ChildId} - {FatherId}", HttpContext.User.FindFirstValue("UserId"), dto.ChildId, dto.FatherId);
        
        await service.ChangeAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Вставка между в составе детали
    /// </summary>
    /// <param name="dto">Параметры для вставки, где 
    /// BeforeDetailNumber - номер детали, перед которой должна встать перемещаемая деталь, 0 если в самое начало 
    /// TargetCurrentDetailNumber - текущий номер детали, которая двигается</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.InsertBetween)]
    public async Task<IActionResult> InsertBetween(InsertBetweenChildDto dto, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Вставка между в составе: {FatherId} - {BeforeDetailNumber} {TargetCurrentDetailNumber}", 
            HttpContext.User.FindFirstValue("UserId"), dto.FatherId, dto.BeforeDetailNumber, dto.CurrentTargetDetailNumber);
        
        await service.InsertBetweenAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Смена деталей в составе местами
    /// </summary>
    /// <param name="dto">Информация для смены местами</param>
    /// <returns>Ok или ошибки с предупреждениями</returns>
    [HttpPost]  
    [Authorize(Roles = DetailControllerRoles.SwapChildrenNumbers)]
    public async Task<IActionResult> SwapChildrenNumbers(DetailChildSwapDto dto, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Свап двух порядковых номеров в деталях {FatherId}: {ChildFirstId} - {ChildSecondId}",
            HttpContext.User.FindFirstValue("UserId"), dto.FatherId, dto.ChildFirstId, dto.ChildSecondId);
        
        await service.SwapChildrenNumbersAsync(dto);
        
        return service.HasWarningsOrErrors ? BadRequest(new { Error = string.Join("\n", service.Errors), service.Warnings }) : Ok();
    }

    /// <summary>
    /// Проверяем список деталей на наличие состава
    /// </summary>
    /// <param name="detailsId">Список Id деталей, которые мы хотим проверить на наличие состава</param>
    /// <returns>Словарь, где Key - detailId, Value - true есть состав, false нет состава или ошибки</returns>
    [HttpPost]
    [Authorize(Roles = DetailControllerRoles.IsCompositions)]
    public async Task<IActionResult> IsCompositions(List<int> detailsId, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Проверка списка деталей на наличие состава", HttpContext.User.FindFirstValue("UserId"));
        
        var result = await service.IsCompositionsAsync(detailsId);

        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok(result);
    }

    /// <summary>
    /// Удаление 
    /// </summary>
    /// <param name="id">Id удаляемой записи</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = DetailControllerRoles.Delete)]
    public async Task<IActionResult> DeleteValue(int id, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление детали: {DetailId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await service.DeleteAsync(id);

        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok();
    }

    /// <summary>
    /// Удаление детали из состава
    /// </summary>
    /// <param name="dto">Информация для удаления</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete]
    [Authorize(Roles = DetailControllerRoles.DeleteChild)]
    public async Task<IActionResult> DeleteChildValue([FromQuery] TwoDetailIdDto dto, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление состава детали: {FatherDetailId} - {ChildDetailId}", HttpContext.User.FindFirstValue("UserId"), dto.FatherDetailId, dto.ChildDetailId);
        
        await service.DeleteAsync(dto);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok();
    }

    /// <summary>
    /// Удаление из заменяемости
    /// </summary>
    /// <param name="id">Id удаляемой детали из заменяемости</param>
    /// <returns>Ok или ошибки</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = DetailControllerRoles.DeleteReplaceability)]
    public async Task<IActionResult> DeleteReplaceability(int id, IDetailReplaceabilitiesService service)
    {
        _logger.LogInformation("Пользователь {userId}: Удаление детали из заменяемости: {DetailId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        await service.DeleteAsync(id);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok();
    }

    /// <summary>
    /// Получение списка деталей
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список деталей с общим количеством или Length = 0</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDetailFilters filters, IDetailService detailService, IDetailCountService countService)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка деталей", HttpContext.User.FindFirstValue("UserId"));
        
        var count = await countService.GetAllAsync(filters);
        
        return count is null or 0 ? Ok(new { Length = 0 }) : Ok(new { Data = await detailService.GetAllAsync(filters), Length = count });
    }

    /// <summary>
    /// Получение списка заменяемости детали
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список заменяемости</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetAllReplaceability)]
    public async Task<ActionResult<List<BaseIdSerialTitleDto>>> GetAllReplaceability([FromQuery] GetAllReplaceabilityFilters filters, IDetailReplaceabilitiesService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка заменяемости детали {DetailId}", HttpContext.User.FindFirstValue("UserId"), filters.DetailId);
        
        return Ok(await service.GetAllAsync(filters));
    }

    /// <summary>
    /// Получаем список применяемости (список изделий)
    /// </summary>
    /// <param name="id">Id детали, чей список изделий мы хотим получить</param>
    /// <returns>Список изделий или ошибки с предупреждениями</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetAllProductDetails)]
    public async Task<ActionResult<List<DetailProductsDto>>> GetAllProducts([FromQuery, Range(1, int.MaxValue)] int id, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка изделий {DetailId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        var products = await service.GetAllProductsAsync(id);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok(products);
    }

    /// <summary>
    /// Получаем весь состав изделия до самого низа дерева
    /// </summary>
    /// <param name="dto">DetailId изделия</param>
    /// <returns>Список состава или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetAllProductDetails)]
    public async Task<ActionResult<List<BaseIdSerialTitleDto>>> GetAllProductDetails([FromQuery] GetAllProductDetailsDto dto, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение списка деталей изделия {ProductDetailId}", HttpContext.User.FindFirstValue("UserId"), dto.DetailId);
        
        var details = await service.GetAllProductDetailsAsync(dto);

        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok(details);
    }

    /// <summary>
    /// Получаем состав уровня ниже
    /// </summary>
    /// <param name="filters">Фильтры выборки</param>
    /// <returns>Список состава</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetAllChildren)]
    public async Task<ActionResult<List<GetDetailChildDto>>> GetAllChildren([FromQuery] GetAllChildrenFilters filters, IDetailChildService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение состава детали {FatherId}", HttpContext.User.FindFirstValue("UserId"), filters.FatherId);
        
        return Ok(await service.GetAllAsync(filters));
    }

    /// <summary>
    /// Подробная информация о детали
    /// </summary>
    /// <param name="id">Id детали</param>
    /// <returns>Подробная информация или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetMoreInfo)]
    public async Task<ActionResult<DetailListDto>> GetMoreInfo([FromQuery, Range(1, int.MaxValue)] int id, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение подробной информации об детали для редактирования: {DetailId}", HttpContext.User.FindFirstValue("UserId"), id);
        
        var detailInfo = await service.GetInfoAsync(id);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok(detailInfo);
    }

    /// <summary>
    /// Получаем информацию о единицы измерения детали
    /// </summary>
    /// <param name="detailId">Id детали, чью единицу измерения мы хотим получить</param>
    /// <returns>Единица измерения или ошибки</returns>
    [HttpGet]
    [Authorize(Roles = DetailControllerRoles.GetDetailUnit)]
    public async Task<IActionResult> GetDetailUnit([FromQuery, Range(1, int.MaxValue)] int detailId, IDetailService service)
    {
        _logger.LogInformation("Пользователь {userId}: Получение единицы измерения детали: {DetailId}", HttpContext.User.FindFirstValue("UserId"), detailId);
        
        var unit = await service.GetDetailUnitAsync(detailId);
        
        return service.HasErrors ? BadRequest(new { Error = string.Join("\n", service.Errors) }) : Ok(unit);
    }
}
