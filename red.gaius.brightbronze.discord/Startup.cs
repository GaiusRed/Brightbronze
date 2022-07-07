using System;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using red.gaius.brightbronze.discord.Models;
using red.gaius.brightbronze.discord.Services;
using red.gaius.brightbronze.core.Models;
using red.gaius.brightbronze.core.Services;

namespace red.gaius.brightbronze.discord
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<CosmosDBSettings>()
                    .Configure<IConfiguration>((cosmosDBSettings, config) =>
                    {
                        config.GetSection("CosmosDB").Bind(cosmosDBSettings);
                    });
            services.AddOptions<ResourceManagerSettings>()
                    .Configure<IConfiguration>((rmSettings, config) =>
                    {
                        config.GetSection("ResourceManager").Bind(rmSettings);
                    });
            services.AddOptions<CacheSettings>()
                    .Configure<IConfiguration>((cacheSettings, config) =>
                    {
                        config.GetSection("Cache").Bind(cacheSettings);
                    });
            services.AddOptions<EngineSettings>()
                    .Configure<IConfiguration>((engineSettings, config) =>
                    {
                        config.GetSection("Engine").Bind(engineSettings);
                    });
            services.AddOptions<DiscordSettings>()
                    .Configure<IConfiguration>((discordSettings, config) =>
                    {
                        config.GetSection("Discord").Bind(discordSettings);
                    });

            services.AddApplicationInsightsTelemetry();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Brightbronze API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Erik Gaius Capistrano",
                        Email = "erik@gaius.red",
                        Url = new Uri("https://gaius.red")
                    }
                });
            });

            services.AddSingleton<IDatastore, CosmosDB>();
            services.AddSingleton<ResourceManager>();
            services.AddSingleton<Cache>();
            services.AddTransient<Engine>();
            services.AddHostedService<DiscordBot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                TelemetryDebugWriter.IsTracingDisabled = true;
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Brightbronze API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
