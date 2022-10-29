using ArcadeBot.Data;
using ArcadeBot.Services;
using Discord.WebSocket;
using Discord;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ArcadeBot.Caching
{
    public class MessageCache : IMessageCache
    {
        private readonly ArcadeBotDbContext _context;
        private readonly DiscordShardedClient _client;
        public ConcurrentDictionary<ulong, IUserMessage> CachedMessages { get; } = new ();

        public MessageCache(ArcadeBotDbContext context, DiscordShardedClient client)
        {
            _context = context;
            _client = client;
        }


        public void Add(ulong key, IUserMessage value)
        {
            CachedMessages.TryAdd(key, value);
            _context.CachedMessages.Add(new UserMessage
            {
                MessageId = value.Id,
                ChannelId = key,
                CreatedAt = value.CreatedAt
            });
            _context.SaveChanges();
        }
        
        public IUserMessage? Get(ulong key, bool ignoreCacheMiss = false)
        {
            if (CachedMessages.TryGetValue(key, out var res) || ignoreCacheMiss) return res;
            throw new KeyNotFoundException($"No Message found for key: [{key}]");
        }

        public async Task<IUserMessage?> GetAsync(ulong key, bool ignoreCacheMiss = false)
        {
            if (CachedMessages.TryGetValue(key, out var res) || ignoreCacheMiss) return res;

            var channel = _client.GetGuild(Constants.MainServerId).GetTextChannel(key);
            if (channel == null)
                return null;
            var msg = (await _context.CachedMessages.ToListAsync()).Where(x => x.ChannelId == channel.Id).MinBy(x => x.CreatedAt);
            if (msg == null)
                return null;
            res = await channel.GetMessageAsync(msg.MessageId) as IUserMessage;
            if (res == null) return null;

            CachedMessages.TryAdd(key, res);

            return res;
        }
        
        public void Remove(ulong key)
        {
            Remove(key, true);
        }
        public void Remove(ulong key, bool skipPersistentCache)
        {
            CachedMessages.TryRemove(key, out _);
            if (skipPersistentCache)
                return;
            var value = _context.CachedMessages.FirstOrDefault(x => x.ChannelId == key);
            if (value == null)
                return;
            _context.CachedMessages.Remove(value);
            _context.SaveChanges();
        }
    }
}
