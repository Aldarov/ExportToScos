namespace Common.Models.Exceptions
{
    /// <summary>
    /// Класс определяет формат ошибок
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        /// <value></value>
        public string? Code { get; set; }

        /// <summary>
        /// Общее сообщение
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Словарь с ошибками: имя свойства, массив ошибок данного свойства
        /// </summary>
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
