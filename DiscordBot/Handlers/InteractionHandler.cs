using ArcadeBot.TypeReaders;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ArcadeBot.Handlers
{
    public class InteractionHandler
    {
        private readonly ILogger<InteractionHandler> _logger;
        private readonly IServiceProvider _services;
        private readonly InteractionService _slashCommands;
        private readonly DiscordShardedClient _client;

        public InteractionHandler(ILogger<InteractionHandler> logger, IServiceProvider services, InteractionService slashCommands, DiscordShardedClient client)
        {
            _logger = logger;
            _services = services;
            _slashCommands = slashCommands;
            _client = client;
            _slashCommands.AddTypeConverter<Uri>(new UriConverter());
        }
        #region InitializeAsync
        public async Task InitializeAsync()
        {
            await _slashCommands.AddModulesAsync(Assembly.GetAssembly(typeof(DiscordBot)), _services);
            _client.ShardReady += ValidateSlashCommandsAsync;

            _client.InteractionCreated += HandleInteraction;
        }

        private async Task ValidateSlashCommandsAsync(DiscordSocketClient client)
        {
            foreach (var guild in client.Guilds)
            {
                //var commands = await guild.GetApplicationCommandsAsync();
                //foreach (var command in commands)
                //{
                //    //if (_slashCommands.SlashCommands.All(x => x.Name != command.Name))
                //    //{
                //    await command.DeleteAsync();
                //    //}
                //}
                await _slashCommands.RegisterCommandsToGuildAsync(guild.Id);
            }
        }


        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new ShardedInteractionContext(_client, arg);
                var res = await _slashCommands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occoured while handling a interaction");
            }
        }
        #endregion
    }
}
