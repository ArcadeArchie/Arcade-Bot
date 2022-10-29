using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeBot.Infrastructure
{
    public interface ICache<TKey, TValue>
    {
        TValue Get(TKey key);
        void Add(TKey key, TValue value);
        void Remove(TKey key);
    }
}
