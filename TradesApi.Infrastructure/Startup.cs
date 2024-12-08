using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TradesApi.Infrastructure;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Repositories;

namespace TradingMicroservice.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TradeDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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
}