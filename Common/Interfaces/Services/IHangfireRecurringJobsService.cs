using Common.Models.Hangfire;

namespace Common.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с повторяющимися заданиями Handfire
    /// </summary>
    public interface IHangfireRecurringJobsService
    {
        /// <summary>
        /// Список повторяющихся заданий Handfire
        /// </summary>
        public IEnumerable<HangfireRecurringJob> Jobs { get; }
    }
}
