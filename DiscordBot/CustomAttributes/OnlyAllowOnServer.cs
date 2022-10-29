using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeBot.CustomAttributes
{
    public class OnlyAllowOnServer : PreconditionAttribute
    {
        private readonly ulong _serverId;
        public OnlyAllowOnServer(ulong serverId)
        {
            _serverId = serverId;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser user)
            {
                if (user.Guild.Id == _serverId)
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
            }

            return Task.FromResult(PreconditionResult.FromError("Sorry this is not allowed on this server"));
        }
    }
}
