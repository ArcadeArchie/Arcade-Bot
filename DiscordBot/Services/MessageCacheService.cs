using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ArcadeBot.Caching;
using ArcadeBot.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ArcadeBot.Services
{
    public class MessageCacheService
    {
        private readonly ILogger<MessageCacheService> _logger;
        private readonly MessageCache _cache;

        public MessageCacheService(MessageCache cache, ILogger<MessageCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }


        public void Add(ulong key, IUserMessage value) => 
            _cache.Add(key, value);

        public async Task<IUserMessage?> GetAsync(ulong key, bool ignoreCacheMiss = false)
        {
            try
            {
                var res = await _cache.GetAsync(key, ignoreCacheMiss);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving message from cache");
            }
            return null;
        }

        public void Remove(ulong key, bool skipPersistentCache) => _cache.Remove(key, skipPersistentCache);
    }

    internal static class SvcEx
    {
        public static void AddMessageCaching(this IServiceCollection services)
        {
            services
                .AddDbContext<ArcadeBotDbContext>()
                .AddScoped<MessageCache>()
                .AddScoped<MessageCacheService>();
        }
    }

    public class UserMessage
    {
        [Key]
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
