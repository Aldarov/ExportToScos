using System.Linq.Expressions;

namespace Common.Models.Hangfire
{
    /// <summary>
    /// Интерфейс повторяющегося задания Handfire
    /// </summary>
    public class HangfireRecurringJob
    {
        /// <summary>
        /// Идентификатор задания
        /// </summary>
        public string? JobId { get; set; }

        /// <summary>
        /// Cron-выражение, задает расписание запуска
        /// </summary>
        public string? CronExpression { get; set; }

        /// <summary>
        /// Функция, кот будет запускаться по расписанию
        /// </summary>
        /// <returns></returns>
        public Expression<Action>? MethodCall { get; set; }
    }
}
