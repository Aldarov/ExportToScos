namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель ошибки после выгрузки данных в ГИС СЦОС
    /// </summary>
    public class ExportError
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Имя сущности
        /// </summary>
        public string? Entity { get; set; }

        /// <summary>
        /// Вариант действия
        /// </summary>
        public string? ActionName { get; set; }

        /// <summary>
        /// Идентификатор записи сущности в системе Университет
        /// </summary>
        public string? ExternalId { get; set; }

        /// <summary>
        /// Идентификатор записи сущности в ГИС СЦОС
        /// </summary>
        public string? ScosId { get; set; }

        /// <summary>
        /// Идентификатор ошибки, общий для всех записей с одинаковым ExternalId и ActionName
        /// </summary>
        public string? ErrorId { get; set; }

        /// <summary>
        /// Идентификатор пакета, при помощи кот. была экспортирована данная запись
        /// </summary>
        public int ExportQueueId { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Json с данными по ошибке
        /// </summary>
        public string? Json { get; set; }

        /// <summary>
        /// Дата получения ошибки
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Если true, то ошибке исправлена
        /// </summary>
        public bool Resolved { get; set; }
    }
}