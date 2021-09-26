using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.discord.Models;

namespace red.gaius.brightbronze.discord.Services
{
    public class DiscordBot : BackgroundService
    {
        readonly ILogger<DiscordBot> _logger;
        readonly DiscordSettings _settings;
        readonly DiscordSocketClient _client;
        public DiscordBot(IServiceProvider services,
                          ILogger<DiscordBot> logger,
                          IOptions<DiscordSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;

            _client = new DiscordSocketClient();
            _client.Log += (log) =>
            {
                if (log.Severity == LogSeverity.Critical)
                    _logger.LogCritical(log.Exception, log.Message);
                if (log.Severity == LogSeverity.Debug)
                    _logger.LogDebug(log.Exception, log.Message);
                if (log.Severity == LogSeverity.Error)
                    _logger.LogError(log.Exception, log.Message);
                if (log.Severity == LogSeverity.Info)
                    _logger.LogInformation(log.Exception, log.Message);
                if (log.Severity == LogSeverity.Verbose)
                    _logger.LogTrace(log.Exception, log.Message);
                if (log.Severity == LogSeverity.Warning)
                    _logger.LogWarning(log.Exception, log.Message);
                return Task.CompletedTask;
            };

            CommandService commandSvc = new CommandService();
            commandSvc.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            _client.MessageReceived += async (arg) =>
            {
                var msg = arg as SocketUserMessage;
                if (msg == null) return;
                int pos = 0;
                if (msg.HasCharPrefix(_settings.DefaultPrefix, ref pos) ||
                    msg.HasMentionPrefix(_client.CurrentUser, ref pos))
                    await commandSvc.ExecuteAsync(
                        new SocketCommandContext(_client, msg), pos, services);
            };

            _client.LoginAsync(TokenType.Bot, _settings.Token);
            _client.StartAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(_settings.Delay, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.StopAsync();
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _client.Dispose();
            base.Dispose();
        }
    }
}
