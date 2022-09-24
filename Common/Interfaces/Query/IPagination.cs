namespace Common.Interfaces.Query
{
    /// <summary>
    /// Интерфейс пагинации списков
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Кол-во записей на странице
        /// </summary>
        public int PageSize { get; set; }
    }
}