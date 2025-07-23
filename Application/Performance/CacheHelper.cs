using System;
using System.Collections.Concurrent;

namespace Terret_Billing.Application.Performance
{
    public static class CacheHelper
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new();
        public static T GetOrAdd<T>(string key, Func<T> valueFactory)
        {
            return (T)_cache.GetOrAdd(key, _ => valueFactory());
        }
        public static void Clear(string key) => _cache.TryRemove(key, out _);
    }
}
