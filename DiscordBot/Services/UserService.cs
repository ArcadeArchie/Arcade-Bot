using ArcadeBot.Data;
using ArcadeBot.Infrastructure.Entities;
using Discord;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace ArcadeBot.Services
{
    public class UserService
    {
        private readonly ArcadeBotDbContext _dbContext;


        public UserService(ArcadeBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ServerUser>> GetUsers(ulong guildId)
        {
            var users = (await _dbContext.Users.ToListAsync()).Where(x => x.Guilds.Any(y => y.Id == guildId));
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<IGuildUser>> GetUsersWithRole(IGuild guild, ulong roleId)
        {
            var users = (await guild.GetUsersAsync()).Where(x => x.RoleIds.Contains(roleId));
            return users;
        }

        public async Task AddRangeAsync(Server? currentServer, IEnumerable<ServerUser> users)
        {
            if (currentServer != null && users.Any())
            {
                foreach (var user in users)
                {
                    if (_dbContext.Users.Any(x => x.Id == user.Id)) continue;
                    user.Guilds = new List<Server?> { currentServer };
                    await _dbContext.Users.AddAsync(user);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
