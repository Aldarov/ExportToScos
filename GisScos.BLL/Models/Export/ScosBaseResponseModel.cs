using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель базового ответа сервиса ГИС СЦОС
    /// </summary>
    public class ScosBaseResponseModel
    {
        /// <summary>
        /// Идентификатор ГИС СЦОС
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Внутрений идентификатор из ИС Вуза
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string? ExternalId { get; set; }

        /// <summary>
        /// Статус отправки
        /// </summary>
        public ScosUploadStatusTypes? UploadStatusType { get; set; }

        /// <summary>
        /// Информация об успешной отправке или ошибке
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string? AdditionalInfo { get; set; }
    }
}
