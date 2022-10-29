using System;
using System.Collections.Generic;
using System.Text;

namespace ArcadeBot
{
    public static class Constants
    {
        public static readonly string[] StaticConfigValues =
        {
            "Token",
            "DbConnectionsString",
            "AdminPermissionRoles",
            "EnabledModules",
            "MemeFolder",
            "PermissionRoles"
        };

        public const string ErrLogMsgTemplate = "Error msg: {message}";
        public const string ErrLogCmdFail = "Command failed to execute for [{username}] <-> [{errorReason}]!";
        public const string ErrLogCmdExecFali = "Error while executing command: {name}, {reason}";
        public const string InfLogCmdExec = "Command [{cmdName}] executed for [{username}] on [{guildName}]";
        public const string ShardedInfLogCmdExec = "Shard {shardId} executed [{cmdName}] for [{username}] on [{guildName}]";
        public const ulong MainServerId = 483551326232641544;
    }
}
