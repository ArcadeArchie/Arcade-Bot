using Discord;
using Discord.Net.Rest;
using Discord.Net.WebSockets;
using Discord.WebSocket;
using ArcadeBot.Infrastructure.Entities;
using ArcadeBot.Handlers;
using ArcadeBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ArcadeBot.Data;
using MediatR;
using Microsoft.Extensions.Options;
using System;

namespace ArcadeBot
{
    public class DiscordBot
    {
        private const GatewayIntents DefaultIntents =
            (GatewayIntents.AllUnprivileged & ~(GatewayIntents.GuildScheduledEvents | GatewayIntents.GuildInvites)) |
            GatewayIntents.GuildMembers | GatewayIntents.GuildMessages | GatewayIntents.MessageContent;
        #region Methods

        #region ConfigureServices
        public static IServiceCollection ConfigureServices(IServiceCollection? platformServices = null)
        {
            IServiceCollection services = platformServices ?? new ServiceCollection();

            _ = services
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            ServiceProvider sv = platformServices.BuildServiceProvider();
            var botConfig = sv.GetRequiredService<IOptions<BotConfig>>().Value;
            DiscordSocketConfig discordConfig = new()
            {
                GatewayIntents = DefaultIntents,
                AlwaysDownloadUsers = true,
                TotalShards = botConfig.MaxShards
            };
            //Create a config for when the user needs proxy support
            if (botConfig.UseProxy)
            {
                _ = botConfig.ProxyHost ?? throw new InvalidOperationException("Proxy Host cannot be null, when using proxy support");
                _ = botConfig.ProxyPort ?? throw new InvalidOperationException("Proxy Port cannot be null, when using proxy support");

                discordConfig.RestClientProvider = DefaultRestClientProvider.Create(botConfig.UseProxy);
                discordConfig.WebSocketProvider = DefaultWebSocketProvider.Create(new WebProxy(botConfig.ProxyHost, botConfig.ProxyPort.Value));
            }

            DiscordShardedClient client = new(discordConfig);
            _ = services
                .AddSingleton(client)
                .AddSingleton(new Discord.Interactions.InteractionService(client));

            services
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddMessageCaching();

            _ = services
                .AddDbContext<ArcadeBotDbContext>()
                .AddScoped<UserService>()
                .AddScoped<ServerService>()
                .AddSingleton<InteractionHandler>();
            return services;
        }

        #endregion

        #endregion
    }
}
