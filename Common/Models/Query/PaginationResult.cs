using Common.Interfaces.Query;

namespace Common.Models.Query
{
    /// <summary>
    /// Информация о пагинации
    /// </summary>
    public class PaginateInfo: IPagination
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Кол-во записей на странице, если равно 0, то возвращен весь набор данных
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее кол-во записей
        /// </summary>
        public int RowCount { get; set; }
    }

    /// <summary>
    /// Класс содержит результирующий набор данных, а также информацию о пагинации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationResult<T>
    {
        /// <summary>
        /// Данные
        /// </summary>
        /// <value></value>
        public IEnumerable<T>? Rows { get; set; }

        /// <summary>
        /// Информация о пагинации
        /// </summary>
        public PaginateInfo? Info { get; set; }
    }
}