using Common.Aspnet.Middlewares;
using GisScos.BLL;
using GisScos.BLL.Models.Settings;
using GisScos.Infrastructure.Authentication;
using GisScos.Infrastructure.Filters;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text.Json.Serialization;

namespace GisScos
{
    /// <summary>
    /// Класс стартовой конфигурации приложения
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Добавление сервисов в контейнер внедрения зависимостей приложения
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.Configure<AppSettings>(configuration);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //отключение стандартной обработки ошибок в ModelState
                options.SuppressModelStateInvalidFilter = true;
            });

            services
                .AddControllers(options =>
                {
                    //ручная обработка ошибок
                    options.Filters.Add(typeof(ThrowModelStateExceptionFilter));
                    options.Filters.Add(typeof(APIExceptionFilterAttribute));
                })
                .AddJsonOptions(opt =>
                {
                    //включения преобразования Enum в строку и обратно, при работе с json
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Сервис экспорта данных в ГИС СЦОС", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "GisScos.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "GisScos.BLL.xml"));
            });

            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            builder.Host.UseNLog();

            services.AddServices();
        }

        /// <summary>
        /// Добавление middlewares в конвейер запросов ASP.NET Core
        /// </summary>
        /// <param name="app">Конвейер запросов</param>
        /// <param name="env">Информация об окружении</param>
        public static void AddMiddlewares(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.UseHangfireJobs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapFallbackToFile("index.html");
        }
    }
}
