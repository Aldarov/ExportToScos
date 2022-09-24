namespace GisScos.BLL.Models.Export
{
    /// <summary>
    /// Модель сущности, экпортируемой в ГИС СЦОС
    /// </summary>
    public class ExportEntityModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Url-имя сущности
        /// </summary>
        public string? UrlName { get; set; }

        /// <summary>
        /// Имя процедуры слияния
        /// </summary>
        public string? MergeProcedure { get; set; }

        /// <summary>
        /// Порядок отправки данных
        /// </summary>
        public int SortNum { get; set; }
    }
}
