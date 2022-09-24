using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Тип результата отправки данных в ГИС СЦОС
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ScosUploadStatusTypes
    {
        /// <summary>
        /// Данные отправлены
        /// </summary>
        Ok,
        /// <summary>
        /// Ошибка при отправке данных
        /// </summary>
        Failed
    }
}
