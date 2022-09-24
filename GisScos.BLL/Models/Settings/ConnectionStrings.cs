namespace GisScos.BLL.Models.Settings
{
    /// <summary>
    /// Класс строк подключения
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// Основная строка подключения
        /// </summary>
        public string? MainConnection { get; set; }

        /// <summary>
        /// Строка подключения Hangfire
        /// </summary>
        public string? HangfireConnection { get; set; }
    }
}
