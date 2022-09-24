using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель ответа для сущности StudyPlansStudents сервиса ГИС СЦОС
    /// </summary>
    public class StudyPlansStudentsResponseModel : ScosBaseResponseModel
    {
        /// <summary>
        /// Идентификатор учебного плана в системе "Университет"
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string? StudyPlan { get; set; }

        /// <summary>
        /// Идентификатор студента в системе "Университет"
        /// </summary>
        public string? Student { get; set; }
    }
}
