using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.ExtensionMethods
{
    /// <summary>
    /// Класс для сериализации объекта в json-формат
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Сериализация объекта в json-формат
        /// </summary>
        /// <typeparam name="T">исходный тип объект</typeparam>
        /// <param name="obj">исходный объект</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) where T : class
        {
            var jsonSetting = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            return JsonConvert.SerializeObject(obj, jsonSetting);
        }
    }
}
