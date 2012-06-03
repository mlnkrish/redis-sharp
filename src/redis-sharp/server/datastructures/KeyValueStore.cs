using System.Collections.Concurrent;

namespace redis_sharp.server.datastructures
{
    public class KeyValueStore
    {
        private readonly ConcurrentDictionary<string,RedisObject> store = new ConcurrentDictionary<string, RedisObject>();
 
        public void Set(string key, RedisObject value)
        {
            store[key] = value;
        }

        public RedisObject Get(string key)
        {
            RedisObject value;
            store.TryGetValue(key, out value);
            return value;
        }
    }
}