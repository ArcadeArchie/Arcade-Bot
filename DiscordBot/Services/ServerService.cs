using ArcadeBot.Data;
using ArcadeBot.Infrastructure.Entities;
using Discord;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ArcadeBot.Services
{
    public class ServerService
    {
        private readonly ArcadeBotDbContext _dbContext;


        public ServerService(ArcadeBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetDefaultRoleAsync(IGuild currentServer, IRole defaultRole)
        {
            var dbServer = await GetServerAsync(currentServer);

            dbServer.DefaultRole = defaultRole.Id;
            _dbContext.Update(dbServer);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentServer"></param>
        /// <returns></returns>
        public async Task<Server> GetServerAsync(IGuild currentServer)
        {
            var dbServer = await _dbContext.Servers.FirstOrDefaultAsync(x => x.Id == currentServer.Id);

            if (dbServer != null) return dbServer;
            dbServer = new Server
            {
                Id = currentServer.Id,
                Name = currentServer.Name
            };
            await _dbContext.AddAsync(dbServer);
            await _dbContext.SaveChangesAsync();
            return dbServer;
        }
    
        public async Task SetQuarantineChannelAsync(Server? currentServer, ulong notificationChannelId)
        {
            if (currentServer != null)
            {
                currentServer.QuarantineChannel = notificationChannelId;
                await _dbContext.SaveChangesAsync();
            }
        }
        
        public async Task<bool> SetQuarantineStatusAsync(Server currentServer, bool status, QuarantineHandleBehavior handleBehavior)
        {
            currentServer.QuarantineEnabled = status;
            currentServer.QuarantineHandleBehavior = handleBehavior;
            await _dbContext.SaveChangesAsync();
            return currentServer.QuarantineEnabled;
        }

        public async Task SetConfigOverrideAsync(Server? currentServer, string configKey, string value)
        {
            if (currentServer != null)
            {
                var configOverride = currentServer.ConfigOverrides?.FirstOrDefault(x => x.ConfigKey == configKey);
                if (configOverride == null)
                {
                    configOverride = new ServerConfigOverride
                    {
                        Key = $"{currentServer.Id}_{configKey}",
                        Server = currentServer,
                        ConfigKey = configKey,
                        OverrideValue = value,
                        Active = true
                    };
                    _dbContext.ServerConfigOverride.Add(configOverride);
                }
                else
                {
                    configOverride.OverrideValue = value;
                }
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<ServerConfigOverride?> GetConfigOverrideAsync(Server? currentServer, string configKey)
        {
            if (currentServer == null) return null;
            var configOverride = await _dbContext
                .ServerConfigOverride.FirstOrDefaultAsync(x => x.Active && x.Key == $"{currentServer.Id}_{configKey}");
            return configOverride;
        }

        public async Task SetQuarantineRoleAsync(Server? currentServer, ulong quarantineRoleId)
        {
            if (currentServer != null)
            {
                currentServer.QuarantineRole = quarantineRoleId;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
