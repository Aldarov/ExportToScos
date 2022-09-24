namespace Common.Interfaces.Query
{
    /// <summary>
    /// Интерфейс для реализации поиска
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string Search { get; set; }
    }
}