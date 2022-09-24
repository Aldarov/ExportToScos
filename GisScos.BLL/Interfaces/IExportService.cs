using Common.Models.Query;
using GisScos.BLL.Models.Export;

namespace GisScos.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса экспорта данных в ГИС СЦОС
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Заполнение очереди и отправка данных в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        Task QueueFillingAndExportData();

        /// <summary>
        /// Возвращает список ошибок после выгрузки данных в ГИС СЦОС
        /// </summary>
        /// <param name="query">Аргументы запроса</param>
        /// <returns></returns>
        Task<PaginationResult<ExportError>> GetExportErrors(QueryModel query);

        /// <summary>
        /// Установить статус решения ошибки
        /// </summary>
        /// <param name="errorId">Код ошибки</param>
        /// <param name="resolved">Если true, то ошибка исправлена</param>
        /// <returns></returns>
        Task SetErrorResolutionStatus(string errorId, bool resolved);
    }
}