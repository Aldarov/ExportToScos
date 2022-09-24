using System.Text;

namespace Common.ExtensionMethods
{
    /// <summary>
    /// Класс для работы с форматом Base64
    /// </summary>
    public static class Base64Converter
    {

        /// <summary>
        /// Получить строку в Base64 формате
        /// </summary>
        /// <param name="value">utf8 строка</param>
        /// <returns></returns>
        public static string EncodeToBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        /// <summary>
        /// Расшифровать данные в формате Base64
        /// </summary>
        /// <param name="value">Base64 строка</param>
        /// <returns></returns>
        public static string DecodeFromBase64(this string value)
        {
            var valueBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
