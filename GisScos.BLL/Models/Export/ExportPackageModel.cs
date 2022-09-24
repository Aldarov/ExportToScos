namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель экспортируемого пакета в очереди для отправки данных в ГИС СЦОС
    /// </summary>
    public class ExportPackageModel
    {
        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Url для экспорта сущности в ГИС СЦОС
        /// </summary>
        public string? EntityExportUrl { get; set; }

        /// <summary>
        /// Имя таблицы сущности с уже выгруженными данными в ГИС СЦОС
        /// </summary>
        public string? EntityTableName { get; set; }

        /// <summary>
        /// Имя json-объект сущности для выгрузки в ГИС СЦОС
        /// </summary>
        public string? JsonObjectName { get; set; }

        /// <summary>
        /// Код статуса отправки
        /// </summary>
        public int ExportStatusId { get; set; }

        /// <summary>
        /// Тип запроса для отправки
        /// </summary>
        public ExportActionTypes ActionName { get; set; }

        /// <summary>
        /// Данные для отправки в формате json
        /// </summary>
        public string? ExportData { get; set; }

        /// <summary>
        /// Ответ от сервера ГИС СЦОС после отправки
        /// </summary>
        public string? ResponseData { get; set; }

        /// <summary>
        /// Дата создания пакета
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
