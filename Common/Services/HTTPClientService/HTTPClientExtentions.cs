
using System.Text;

namespace Common.Services.HTTPClientService
{
    /// <summary>
    /// Класс методов расширения для HTTPClientService
    /// </summary>
    public static class HTTPClientExtentions
    {
        /// <summary>
        /// Добавляет заголовки в запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="headers">Заголовки</param>
        public static void AddHeaders(this HttpRequestMessage request, Dictionary<string, string>? headers)
        {
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Добавляет Json в тело запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="json">Json-данные</param>
        public static void AddJsonContent(this HttpRequestMessage request, string? json)
        {
            if (json != null)
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }
    }
}
