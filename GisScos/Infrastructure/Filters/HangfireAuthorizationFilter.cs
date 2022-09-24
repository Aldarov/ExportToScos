using System.Linq;
using System.Security.Claims;
using Hangfire.Dashboard;

namespace GisScos.Infrastructure.Filters
{
    /// <summary>
    /// Класс настройки фильтра авторизации Handfire
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <param name="context">DashboardContext</param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var identity = httpContext?.User?.Identity as ClaimsIdentity;
            var isAdmin = identity?.FindAll(x => x.Type == ClaimTypes.Role && x.Value == "admin").Any() ?? false;
            return (identity?.IsAuthenticated ?? false) && isAdmin;
        }
    }
}
