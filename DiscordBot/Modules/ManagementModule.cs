using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcadeBot.Services;
using Discord;
using Discord.WebSocket;
using MediatR;
using System.Threading;
using ArcadeBot.Infrastructure.Entities;

namespace ArcadeBot.Modules
{
    [Group("quarantine", "Commands for the Quarantine feature")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class QuarantineManagementModule : InteractionModuleBase<ShardedInteractionContext>, INotificationHandler<MessageReceived>
    {
        private readonly ServerService _serverService;
        private readonly DiscordShardedClient _client;
        private readonly IMediator _mediator;

        public QuarantineManagementModule(ServerService serverService, DiscordShardedClient client, IMediator mediator)
        {
            _serverService = serverService;
            _client = client;
            _mediator = mediator;

            _client.MessageReceived += Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg.Author.IsBot)
                return;
            switch (arg)
            {
                case SocketUserMessage message:
                    await _mediator.Publish(new MessageReceived
                    {
                        Actual = message,
                        Server = (message.Channel as SocketTextChannel)!.Guild
                    });
                    break;
            }
        }

        [SlashCommand("channel", "set the channel used for Quarantine")]
        public async Task SetQuarantineChannel(ITextChannel quarantineChannel)
        {
            await DeferAsync(ephemeral: true);

            var server = await _serverService.GetServerAsync(Context.Guild);

            await _serverService.SetQuarantineChannelAsync(server, quarantineChannel.Id);

            await ModifyOriginalResponseAsync(props => props.Content = $"Quarantine channel successfully set to <#{quarantineChannel.Id}>");
        }

        [SlashCommand("role", "set the role used for Quarantine")]
        public async Task SetQuarantineChannel(IRole quarantineRole)
        {
            await DeferAsync(ephemeral: true);

            var server = await _serverService.GetServerAsync(Context.Guild);

            await _serverService.SetQuarantineRoleAsync(server, quarantineRole.Id);

            await ModifyOriginalResponseAsync(props => props.Content = $"Quarantine role successfully set to <@&{quarantineRole.Id}>");
        }

        [DefaultMemberPermissions(GuildPermission.ManageRoles | GuildPermission.ManageMessages |
                GuildPermission.CreatePublicThreads | GuildPermission.KickMembers | GuildPermission.BanMembers)]
        [SlashCommand("enable", "Enable the Quarantine feature")]
        public async Task EnableQuarantine(QuarantineHandleBehavior handleBehavior)
        {
            await DeferAsync(ephemeral: true);

            var server = await _serverService.GetServerAsync(Context.Guild);

            if (server.QuarantineChannel == 0ul && handleBehavior == QuarantineHandleBehavior.Quarantine)
            {
                await ModifyOriginalResponseAsync(props => props.Content = $"A quarantine channel must be set first before enabling");
                return;
            }

            if (server.QuarantineRole == 0ul && handleBehavior == QuarantineHandleBehavior.Quarantine)
            {
                await ModifyOriginalResponseAsync(props => props.Content = $"A quarantine role must be set first before enabling");
                return;
            }

            var res = await _serverService.SetQuarantineStatusAsync(server, true, handleBehavior);

            await ModifyOriginalResponseAsync(props => props.Content = $"Quarantine is now enabled with behavior {handleBehavior}");
        }

        [SlashCommand("disable", "Disable the Quarantine feature")]
        public async Task DisableQuarantine()
        {
            await DeferAsync(ephemeral: true);

            var server = await _serverService.GetServerAsync(Context.Guild);

            var res = await _serverService.SetQuarantineStatusAsync(server, false, 0);

            await ModifyOriginalResponseAsync(props => props.Content = $"Quarantine is now disabled");
        }

        [SlashCommand("defaultrole", "Sets the default role for this server")]
        public async Task SetDefaultServerRole(IRole defaultRole)
        {
            await DeferAsync(ephemeral: true);
            var currentServer = Context.Guild;
            await _serverService.SetDefaultRoleAsync(currentServer, defaultRole);
            await ModifyOriginalResponseAsync(props => props.Content = "👌");
        }



        /// <summary>
        /// Handles all incoming messages and checks them for quarantine liability 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MessageReceived notification, CancellationToken cancellationToken)
        {
            var message = notification.Actual;
            if (!message.Content.Contains("discord.gg/"))
                return;

            var server = await _serverService.GetServerAsync(notification.Server);
            if (!server.QuarantineEnabled)
                return;

            await message.Channel.SendMessageAsync($"<@{message.Author.Id}>, do not send discord invite links, punishment is underway.");


            var user = notification.Server.GetUser(message.Author.Id);
            if (user == null) return;

            switch (server.QuarantineHandleBehavior)
            {
                case QuarantineHandleBehavior.Ban:
                    await user.BanAsync();
                    return;
                case QuarantineHandleBehavior.Kick:
                    await message.DeleteAsync();
                    await user.KickAsync();
                    return;
                case QuarantineHandleBehavior.DeleteMessage:
                    await message.DeleteAsync();
                    return;
                case QuarantineHandleBehavior.Quarantine:
                default:
                    break;
            }
            
            await message.DeleteAsync();
            await user.AddRoleAsync(server.QuarantineRole);
            await user.RemoveRoleAsync(server.DefaultRole);

            var quarantineChannel = notification.Server.GetTextChannel(server.QuarantineChannel);
            var incidentChannel = await quarantineChannel.CreateThreadAsync($"Incident {Guid.NewGuid()}");
            await incidentChannel.AddUserAsync(user);
        }
    }

    public class MessageReceived : INotification
    {
        public SocketUserMessage Actual { get; set; } = null!;
        public SocketGuild Server { get; set; } = null!;
    }

    internal class NewUserJoined : INotification
    {
        public SocketGuildUser NewUser { get; set; } = null!;
    }
}
