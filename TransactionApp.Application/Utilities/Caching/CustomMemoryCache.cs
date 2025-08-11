using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace TransactionApp.Application.Utilities.Caching
{
    public class CustomMemoryCache : ICustomCache
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, byte> _keys = new();

        public CustomMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
            _keys.TryAdd(key, 0);
        }

        public bool TryGetValue<T>(string key, out T value) 
            => _cache.TryGetValue(key, out value);

        public void RemoveByKey(string key)
        {
            _cache.Remove(key);
            _keys.TryRemove(key, out _);
        }
    }
}
