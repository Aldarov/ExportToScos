using Common.Interfaces.Services;
using System.Text.RegularExpressions;

namespace Common.Services.HTTPClientService
{
    /// <summary>
    /// Сервис для работы со сторонними rest-сервисами
    /// </summary>
    public class HTTPClientService : IHTTPClientService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        public HTTPClientService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Возвращает ответ, полученный после отправки запроса
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> CreateResponse(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(Regex.Unescape(data));
            }

            return data;
        }

        /// <summary>
        /// Запрос на получение данных
        /// </summary>
        public async Task<string> Get(string url, Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddHeaders(headers);
            var response = await _httpClient.SendAsync(request);
            return await CreateResponse(response);
        }

        /// <summary>
        /// Запрос на добавление данных
        /// </summary>
        public async Task<string> Post(string url, string? json, Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.AddHeaders(headers);
            request.AddJsonContent(json);
            var response = await _httpClient.SendAsync(request);

            return await CreateResponse(response);
        }

        /// <summary>
        /// Запрос на изменение данных
        /// </summary>
        public async Task<string> Put(string url, string? json, Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.AddHeaders(headers);
            request.AddJsonContent(json);
            var response = await _httpClient.SendAsync(request);

            return await CreateResponse(response);
        }

        /// <summary>
        /// Запрос на удаление данных
        /// </summary>
        public async Task<string> Delete(string url, Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.AddHeaders(headers);
            var response = await _httpClient.SendAsync(request);

            return await CreateResponse(response);
        }
    }
}
