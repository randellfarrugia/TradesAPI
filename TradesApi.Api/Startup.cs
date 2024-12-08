using Microsoft.EntityFrameworkCore;
using TradesApi.Core.Services;
using TradesApi.Infrastructure;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Repositories;
using TradesApi.Infrastructure.Services;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TradeDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<MessageQueueService>();
        services.AddScoped<ITradesRepository, TradesRepository>();
        services.AddScoped<ITradesService, TradesService>();

        services.AddControllers();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
