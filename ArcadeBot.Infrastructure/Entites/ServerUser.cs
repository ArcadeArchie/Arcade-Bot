using System;
using System.Collections.Generic;

namespace ArcadeBot.Infrastructure.Entities
{
    public class ServerUser
    {
        public ulong Id { get; set; }

        public virtual IEnumerable<Server> Guilds { get; set; }

        public string Discriminator { get; set; }

        public bool IsBot { get; set; }

        public string Username { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

    }
}
