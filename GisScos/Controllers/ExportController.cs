using Common.Models.Query;
using GisScos.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GisScos.Controllers
{
    /// <summary>
    /// Контроллер экпорта данных в ГИС СЦОС
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExportController : Controller
    {
        private readonly IExportService _exportService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        /// <summary>
        /// Возвращает список ошибок после выгрузки данных в ГИС СЦОС
        /// </summary>
        /// <param name="query">Аргументы запроса</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-errors")]
        public async Task<IActionResult> GetExportErrors([FromQuery]QueryModel query)
        {
            var res = await _exportService.GetExportErrors(query);
            return Ok(res);
        }

        /// <summary>
        /// Установить статус решения ошибки
        /// </summary>
        /// <param name="errorId">Код ошибки</param>
        /// <param name="resolved">Если true, то ошибка исправлена</param>
        /// <returns></returns>
        [HttpGet]
        [Route("set-error-resolution-status")]
        public async Task<IActionResult> SetErrorResolutionStatus(string errorId, bool resolved)
        {
            await _exportService.SetErrorResolutionStatus(errorId, resolved);
            return Ok();
        }
    }
}
