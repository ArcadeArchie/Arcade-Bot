using System;
using System.Collections.Generic;
using System.Text;

namespace ArcadeBot.Infrastructure.Entities
{
    public class Server
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ulong AdminRole { get; set; }
        public ulong DefaultRole { get; set; }
        public bool QuarantineEnabled { get; set; }
        public ulong QuarantineRole { get; set; }
        public ulong QuarantineChannel { get; set; }        
        public QuarantineHandleBehavior QuarantineHandleBehavior { get; set; }
        public virtual IEnumerable<ServerUser> Users { get; set; }
        public virtual IEnumerable<ServerConfigOverride> ConfigOverrides { get; set; }
    }    
    public enum QuarantineHandleBehavior
    {
        DeleteMessage = 4,
        Ban = 2,
        Kick = 1,
        Quarantine = 0
    }
}
