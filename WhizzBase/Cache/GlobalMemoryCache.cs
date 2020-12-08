using System.Collections.Concurrent;
using System.Linq;
using WhizzBase.Reflection;

namespace WhizzBase.Cache
{
    public static class GlobalMemoryCache
    {
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();

        public static TValue Get<TKey, TValue>(TKey key) 
            where TKey : class
            where TValue : class, new()
        {
            return (TValue) Cache[EncodeKey(key)];
        }
        
        public static void AddOrUpdate<TKey, TValue>(TKey key, TValue value)
            where TKey : class
            where TValue : class, new()
        {
            Cache[EncodeKey(key)] = value;
        }

        public static bool ContainsKey<TKey>(TKey key) => Cache.ContainsKey(EncodeKey(key));
        
        private static string EncodeKey<TKey>(TKey key)
            => string.Join("#", ReflectionFactory.GetterDelegatesByPropertyInfo(typeof(TKey)).Values.Select(s => s(key).ToString()));
    }
}
