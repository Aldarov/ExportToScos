namespace Common.Interfaces.Services
{
    /// <summary>
    /// Интерфейс отправки сообщений стороннему сервису
    /// </summary>
    public interface IHTTPClientService
    {
        /// <summary>
        /// Запрос на получение данных
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="headers">Заголовки</param>
        /// <returns></returns>
        Task<string> Get(string url, Dictionary<string, string>? headers);

        /// <summary>
        /// Запрос на добавление данных
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="json">Передаваемые json-данные</param>
        /// <param name="headers">Заголовки</param>
        /// <returns></returns>
        Task<string> Post(string url, string? json, Dictionary<string, string>? headers);

        /// <summary>
        /// Запрос на изменение данных
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="json">Передаваемые json-данные</param>
        /// <param name="headers">Заголовки</param>
        /// <returns></returns>
        Task<string> Put(string url, string? json, Dictionary<string, string>? headers);

        /// <summary>
        /// Запрос на удаление данных
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="headers">Заголовки</param>
        /// <returns></returns>
        Task<string> Delete(string url, Dictionary<string, string>? headers);
    }
}
