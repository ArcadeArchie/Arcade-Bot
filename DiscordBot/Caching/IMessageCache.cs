using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace ArcadeBot.Caching
{
    internal interface IMessageCache : IAsyncCache<ulong, IUserMessage>
    {
        ConcurrentDictionary<ulong, IUserMessage> CachedMessages { get; }
    }
}
