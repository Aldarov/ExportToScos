namespace GisScos.BLL.Models.Settings
{
    /// <summary>
    /// Класс настроек приложения
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Строки подключения
        /// </summary>
        public ConnectionStrings? ConnectionStrings { get; set; }

        /// <summary>
        /// Url хоста ГИС СЦОС
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// Базовый url сервиса экспорта данных в ГИС СЦОС
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Уникальный ключ доступа в систему ГИС СЦОС
        /// </summary>
        public string? AccessKeyToScos { get; set; }

        /// <summary>
        /// Включить выгрузку данных в ГИС СЦОС
        /// </summary>
        /// <value></value>
        public bool EnableExport { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public IEnumerable<UserAuthenticationModel>? Users { get; set; }
    }
}
