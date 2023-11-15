using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Plan7.Helper.Controllers.AbstractControllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shared.Dto.Tools;
using ServiceLayer.Tools.Services.Interfaces;
using DB.Model.ToolInfo;

namespace Plan7.Controllers.Tools
{
    public class ToolCatalogController : BaseReactController<ToolCatalogController>
    {
        private readonly IToolCatalogsService _service;

        public ToolCatalogController(ILogger<ToolCatalogController> logger, IToolCatalogsService services)
        : base(logger)
        {
            _service = services;
        }

        /// <summary>
        /// Создание каталога
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCatalog(AddToolCatalogDto dto)
        {
            _logger.LogInformation("Пользователь {userId}: Добавление каталога {title}",
                HttpContext.User.FindFirstValue("UserId"), dto.Title);

            var id = await _service.AddCatalogAsync(dto);

            return _service.HasWarningsOrErrors
                ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
                : Ok(id);
        }

        /// <summary>
        /// Изменение каталога
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeCatalog(ChangeToolCatalogDto dto)
        {
            _logger.LogInformation("Пользователь {userId}: Изменение каталога {title}",
                HttpContext.User.FindFirstValue("UserId"), dto.Title);

            await _service.ChangeCatalogAsync(dto);

            return _service.HasWarningsOrErrors
                ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
                : Ok();
        }

        /// <summary>
        /// Удаление каталога
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Пользователь {userId}: Удаление каталога {id}",
                HttpContext.User.FindFirstValue("UserId"), id);
            await _service.DeleteCatalogAsync(id);
            return _service.HasWarningsOrErrors
                ? BadRequest(new { Error = string.Join("\n", _service.Errors), _service.Warnings })
                : Ok();
        }

        /// <summary>
        /// Получение подкаталогов
        /// </summary>
        /// <param name="fatherId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GetToolCatalogDto>>> GetLevel([FromQuery, DefaultValue(null)] int? fatherId)
        {
            _logger.LogInformation("Пользователь {userId}: Получение дочерних подкаталогов каталога {Id}",
                HttpContext.User.FindFirstValue("UserId"),fatherId);
            var toolCatalogs = await _service.GetLevelAsync(fatherId);
            return _service.HasWarningsOrErrors 
                ? BadRequest(new { Error = string.Join("\n", _service.Errors) }) 
                : Ok(toolCatalogs);
        }

        /// <summary>
        /// Получение подкаталогов и инструментов
        /// </summary>
        /// <param name="fatherId"></param>
        /// <param name="toolsService"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OpenCatalog([FromQuery] GetAllOpenCatalog dto, [FromServices] IToolsService toolsService)
        {
            _logger.LogInformation("Пользователь {userId}: Открытие каталога {id}",
                HttpContext.User.FindFirstValue("UserId"), dto.FatherId);
            
            return Ok(await _service.AddKeyAsync(dto));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeCatalogChild(ChangeToolAndCatalogChild dto)
        {
            _logger.LogInformation("Пользователь {userId}: Изменение дочерних элементов каталога {id}",
                HttpContext.User.FindFirstValue("UserId"), dto.FatherId);
            await _service.ChangeChildCatalogAsync(dto);
            return _service.HasWarningsOrErrors
                ? BadRequest(new { Error = string.Join("\n", _service.Errors) })
                : Ok();
        }
    }
}
