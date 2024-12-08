using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TradesApi.Core.Services;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Repositories;
using TradesApi.Infrastructure.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MessageQueueService>();

        services.AddScoped<ITradesService, TradesService>();
        services.AddScoped<ITradesRepository, TradesRepository>();

        Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
              .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
              .Enrich.FromLogContext()
              .WriteTo.File(@"Logs\" + DateTime.UtcNow + "\\log-.txt", rollingInterval: RollingInterval.Hour)
              .CreateLogger();

        services.AddLogging(builder => builder.AddSerilog(Log.Logger));
    }
}