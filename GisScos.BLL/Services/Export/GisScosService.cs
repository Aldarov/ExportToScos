

using Common.Interfaces.Services;
using GisScos.BLL.Context;
using GisScos.BLL.Models.Settings;
using Microsoft.Extensions.Options;

namespace GisScos.BLL.Services.Export
{
    /// <summary>
    /// Сервис для получения данных из ГИС СЦОС
    /// </summary>
    public class GisScosService : IGisScosService
    {
        private readonly AppSettings _settings;
        private readonly DapperContext _context;
        private readonly IHTTPClientService _httpClientService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст данных</param>
        /// <param name="settings">Параметры</param>
        /// <param name="httpClientService"></param>
        public GisScosService(DapperContext context, IOptions<AppSettings> settings, IHTTPClientService httpClientService)
        {
            _settings = settings.Value;
            _context = context;
            _httpClientService = httpClientService;
        }

        /// <summary>
        /// Проверка подключения к ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckConnection()
        {
            var response = await _httpClientService.Get(
                $"{_settings?.Host}api/v1/connection/check",
                new Dictionary<string, string>() { { "X-CN-UUID", _settings?.AccessKeyToScos! } }
            );
            return response;
        }
    }
}