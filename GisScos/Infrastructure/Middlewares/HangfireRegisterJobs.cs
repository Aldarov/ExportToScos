using Common.Interfaces.Services;
using Hangfire;

namespace Common.Aspnet.Middlewares
{
    /// <summary>
    /// Класс для регистрации повторяющихся заданий в Handfire
    /// </summary>
    public static class HangfireRegisterJobs
    {
        /// <summary>
        /// Регистрация повторяющихся заданий в Handfire
        /// </summary>
        public static void UseHangfireJobs(this WebApplication app)
        {
            var recurringJobManager = app.Services.GetService<IRecurringJobManager>();
            var handfireRecurringJobsService = app.Services.GetService<IHangfireRecurringJobsService>();

            if (handfireRecurringJobsService?.Jobs != null)
            {
                foreach (var job in handfireRecurringJobsService.Jobs)
                {
                    recurringJobManager?.AddOrUpdate(job.JobId, job.MethodCall, job.CronExpression, timeZone: TimeZoneInfo.Local);
                }
            }
        }
    }
}
