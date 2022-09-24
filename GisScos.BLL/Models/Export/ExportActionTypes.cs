using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Тип действия при отправке данных
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExportActionTypes
    {
        /// <summary>
        /// Добавить данные
        /// </summary>
        Insert = 0,
        /// <summary>
        /// Изменить данные
        /// </summary>
        Update = 1,
        /// <summary>
        /// Удалить данные
        /// </summary>
        Delete = 2
    }
}
