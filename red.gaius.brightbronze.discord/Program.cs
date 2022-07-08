using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace red.gaius.brightbronze.discord
{
    public class Program
    {
        const string appName = "Brightbronze";

        public static int Main(string[] args)
        {
            LoggerConfigurationExtensions.SetupLoggerConfiguration(appName);

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostBuilderContext, services, loggerConfiguration) => {
                    loggerConfiguration.ConfigureBaseLogging(appName);
                    loggerConfiguration.AddApplicationInsightsLogging(services, hostBuilderContext.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
