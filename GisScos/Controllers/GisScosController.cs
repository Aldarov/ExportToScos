using Microsoft.AspNetCore.Mvc;

namespace GisScos.Controllers
{
    /// <summary>
    /// Контроллер для получения данных из ГИС СЦОС
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GisScosController : Controller
    {
        private readonly IGisScosService _gisScosService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public GisScosController(IGisScosService gisScosService)
        {
            _gisScosService = gisScosService;
        }

        /// <summary>
        /// Проверка подключения к ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("check")]
        public async Task<IActionResult> Check()
        {
            var res = await _gisScosService.CheckConnection();
            return Ok(res);
        }
    }
}