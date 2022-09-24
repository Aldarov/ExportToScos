namespace Common.Models.Exceptions
{
    /// <summary>
    /// Класс определяет формат ошибки некорректного запроса
    /// </summary>
    public class ExceptionWithCode : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        /// <value></value>
        public string Code { get; private set; }

        /// <summary>
        /// Определяет формат ошибки некорректного запроса
        /// </summary>
        /// <param name="message">Ошибка</param>
        /// <param name="code">Код ошибки</param>
        /// <returns></returns>
        public ExceptionWithCode(string message, string code) : base(message)
        {
            Code = code;
        }
    }
}
