using Common.Interfaces.Services;
using Common.Models.Hangfire;
using GisScos.BLL.Interfaces;

namespace GisScos.BLL.Services
{
    /// <summary>
    /// Сервис для работы с повторяющимися заданиями Handfire
    /// </summary>
    public class HangfireRecurringJobsService : IHangfireRecurringJobsService
    {
        /// <summary>
        /// Список повторяющихся заданий Handfire
        /// </summary>
        public IEnumerable<HangfireRecurringJob> Jobs { get; }


        /// <summary>
        /// Конструктор
        /// Для составления CronExpression смотри: http://www.nncron.ru/nncronlt/help/RU/working/cron-format.htm
        /// </summary>
        public HangfireRecurringJobsService(IExportService exportService)
        {
            Jobs = new List<HangfireRecurringJob>
            {
                new HangfireRecurringJob()
                {
                    JobId = "queueFillingAndExportData",
                    CronExpression = "0 0 * * 1-6", //каждый день кроме воскресенья в полночь
                    MethodCall = () => exportService.QueueFillingAndExportData()
                }
            };
        }
    }
}
