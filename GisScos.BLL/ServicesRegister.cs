using Common.Interfaces.Services;
using Common.Services.CurlHTTPClientService;
using GisScos.BLL.Context;
using GisScos.BLL.Interfaces;
using GisScos.BLL.Services;
using GisScos.BLL.Services.Export;
using Microsoft.Extensions.DependencyInjection;


namespace GisScos.BLL
{
    /// <summary>
    /// Класс регистратор сервисов
    /// </summary>
    public static class ServicesRegister
    {
        /// <summary>
        /// Добавление сервисов в контейнер внедрения зависимостей
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddTransient<IHangfireRecurringJobsService, HangfireRecurringJobsService>();

            services.AddTransient<IHTTPClientService, CurlHTTPClientService>();
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IGisScosService, GisScosService>();

            return services;
        }
    }
}
