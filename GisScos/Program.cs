using GisScos;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.AddMiddlewares(builder.Environment);

var logger = LogManager
    .Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{
    app.Run();
}
catch (Exception e)
{
    logger.Fatal(e, "Stopped program because of exception");
}
finally
{
    LogManager.Shutdown();
}