using System;
using System.Collections.Generic;
using System.Text;

namespace ArcadeBot.Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string DiscordId { get; set; }
        public int Status { get; set; }
    }

    public enum UserStatus
    {
        Owner = 0,
        Developer = 1,
        Moderator = 3,
        Normal = 4
    }
}
