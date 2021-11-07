using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitFaster.Caching.Lru;
using StackExchange.Redis;
using RedisProxy.Services;

namespace RedisProxy.Services {

    public sealed class LRUCache : ILocalCache {
        private readonly ConcurrentLru<string, string> _lruCache;

        public LRUCache(int capacity) {
            _lruCache = new ConcurrentLru<string, string>(capacity);
        }

        public bool GetKey(string key, out string returnValue) {
            var foundKey = _lruCache.TryGet(key, out var cacheRet);
            returnValue = cacheRet;
            return foundKey;
        }

        public void AddOrUpdateKey(string key, string value) {
            _lruCache.AddOrUpdate(key, value);
        }
        public int Count => this._lruCache.Count;
    }
}