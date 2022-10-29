using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeBot.Caching
{
    internal interface ICache<TKey, TValue> where TKey : notnull
    {
        void Add(TKey key, TValue value);
        void Remove(TKey key);
        TValue? Get(TKey key, bool ignoreCacheMiss = false);
    }
    internal interface IAsyncCache<TKey, TValue> : ICache<TKey, TValue> where TKey : notnull
    {
        Task<TValue?> GetAsync(TKey key, bool ignoreCacheMiss = false);
    }
}
