using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using WormsWorld.model;
using WormsWorld.service;
using WormsWorld.service.@interface;

namespace WormsWorld
{
    static class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<SimulatorHostedService>();
                    services.AddSingleton<FoodService>();
                    services.AddSingleton<IFoodService>(x => x.GetRequiredService<FoodService>());
                    services.AddSingleton<IAiService>(x => x.GetRequiredService<FoodService>());
                    services.AddSingleton<IRemovable>(x => x.GetRequiredService<FoodService>());
                    services.AddSingleton<IWormService, WormService>();
                    services.AddSingleton<INameService, NameService>();
                    services.AddSingleton<IWormAiService, WormAiService>();
                    services.AddSingleton<ISimulator, Simulator>();

                    services.Configure<SimulatorOptions>(context
                        .Configuration.GetSection(nameof(SimulatorOptions)));
                })
                .ConfigureLogging(context =>
                {
                    context.ClearProviders();
                    context.AddNLog();
                });
        }
    }
}