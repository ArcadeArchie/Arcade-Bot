using ArcadeBot;
using ArcadeBot.Handlers;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ArcadeBot.Infrastructure.Entities;
using Microsoft.Extensions.Options;

namespace ArcadeBotStart
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
#if RELEASE
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Warning)
#endif
                .CreateLogger();
            var serviceHost = CreateHostBuilder(args);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                serviceHost.UseSystemd();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                serviceHost.UseWindowsService();

            await serviceHost.Build().RunAsync();
        }


        static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((_, config) =>
            {
                config
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                    .AddJsonFile("config.json", false);
            })
            .ConfigureServices((_, services) =>
            {
                DiscordBot.ConfigureServices(services);
                services.AddHostedService<BotWorker>();

            });
    }

    internal class BotWorker : BackgroundService
    {
        private readonly BotConfig _config;
        private readonly ILogger<BotWorker> _logger;
        private readonly DiscordShardedClient _client;
        private readonly InteractionHandler _interactionHandler;

        public BotWorker(DiscordShardedClient client, InteractionHandler interactionHandler, ILogger<BotWorker> logger, IOptions<BotConfig> config)
        {
            _client = client;
            _interactionHandler = interactionHandler;
            _logger = logger;
            _client.ShardReady += OnReadyAsync;
            _client.Log += OnLogAsync;
            _config = config.Value;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            
            await _interactionHandler.InitializeAsync();

            await Task.Delay(-1, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.StopAsync();

            await base.StopAsync(cancellationToken);
        }

        
        #region OnReadyAsync
        public Task OnReadyAsync(DiscordSocketClient shard)
        {
            _logger.LogInformation($"Shard {shard.ShardId} ready as -> [{shard.CurrentUser}] :)");
            _logger.LogInformation($"We are on [{shard.Guilds.Count}] servers");
            return Task.CompletedTask;
        }
        #endregion

        #region OnLogAsync

        ///<summary>
        /// Logs the given Message to the console
        ///</summary>
        private Task OnLogAsync(LogMessage msg)
        {
            string logText = $": {msg.Exception?.ToString() ?? msg.Message}";
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    {
                        _logger.LogCritical(msg.Exception, logText);
                        break;
                    }
                case LogSeverity.Warning:
                    {
                        _logger.LogWarning(logText);
                        break;
                    }
                case LogSeverity.Info:
                    {
                        _logger.LogInformation(logText);
                        break;
                    }
                case LogSeverity.Verbose:
                    {
                        _logger.LogInformation(msg.Exception, logText);
                        break;
                    }
                case LogSeverity.Debug:
                    {
                        _logger.LogDebug(msg.Exception, logText);
                        break;
                    }
                case LogSeverity.Error:
                    {
                        _logger.LogError(msg.Exception, logText);
                        break;
                    }
            }
            return Task.CompletedTask;
        }
        
        #endregion
    }
}
