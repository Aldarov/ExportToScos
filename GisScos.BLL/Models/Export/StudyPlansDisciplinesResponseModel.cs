using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель ответа для сущности StudyPlansDisciplines сервиса ГИС СЦОС
    /// </summary>
    public class StudyPlansDisciplinesResponseModel: ScosBaseResponseModel
    {
        /// <summary>
        /// Идентификатор учебного плана в системе "Университет"
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string? StudyPlan { get; set; }

        /// <summary>
        /// Идентификатор дисциплины в системе "Университет"
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string? Discipline { get; set; }

        /// <summary>
        /// Номер семестра
        /// </summary>
        public string? Semester { get; set; }
    }
}
