using Common.Interfaces.Query;

namespace Common.Models.Query
{
    public class QueryModel: IPagination, ISearch
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Кол-во записей на странице, если передать 0, то будет возвращен весь набор данных
        /// </summary>
        public int PageSize { get; set; } = 0;

        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string Search { get; set; } = string.Empty;
    }
}