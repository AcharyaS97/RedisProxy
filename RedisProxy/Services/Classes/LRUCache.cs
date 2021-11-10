using System;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitFaster.Caching.Lru;
using StackExchange.Redis;
using RedisProxy.Services;
using System.Timers;
using ConcurrentPriorityQueue;
using ConcurrentPriorityQueue.Core;

namespace RedisProxy.Services {

    public sealed class LRUCache : ILocalCache {

        public class PriorityQueueItem : IHavePriority<DateTime> {

            public PriorityQueueItem(DateTime dt, string k) {
                Priority = dt;
                Key = k;
            }
            public DateTime Priority { get; set; }
            public string Key { get; set; }
        }
        private readonly ConcurrentLru<string, string> _lruCache;
        private readonly ConcurrentPriorityQueue<IHavePriority<DateTime>, DateTime> _expiryQueue;
        private readonly double _expiryInterval_MS;
        private ILogger<LRUCache> _logger;
        private Timer _expiryTimer;

        public LRUCache(int capacity, int expiryTime, ILogger<LRUCache> logger) {
            _logger = logger;

            _expiryInterval_MS = 2000;

            _lruCache = new ConcurrentLru<string, string>(capacity);
            _expiryQueue = new ConcurrentPriorityQueue<IHavePriority<DateTime>, DateTime>();
            _expiryTimer = new Timer();
            _expiryTimer.Elapsed += RemoveExpiredCacheEntries;
            _expiryTimer.Interval = expiryTime;
            _expiryTimer.AutoReset = true;
            _expiryTimer.Start();
        }

        public bool GetKey(string key, out string returnValue) {
            var foundKey = _lruCache.TryGet(key, out var cacheRet);
            returnValue = cacheRet;
            return foundKey;
        }

        public void AddOrUpdateKey(string key, string value) {
            _logger.LogInformation($"Calling AddOrUpdate for key:{key}, value:{value}");

            var keyExists = _lruCache.TryGet(key, out var _);
            _lruCache.AddOrUpdate(key, value);

            if (!keyExists) {
                _logger.LogInformation($"Key {key} is new, so it will be added to the expiry queue");
                AddKeyToExpiryQueue(key);
            }
        }

        private void AddKeyToExpiryQueue(string key) {
            var expiryTime = DateTime.UtcNow.AddMilliseconds(_expiryInterval_MS);
            _expiryQueue.Enqueue(new PriorityQueueItem(expiryTime, key));
        }

        private void RemoveExpiredCacheEntries(Object _, ElapsedEventArgs __) {
            var currentTime = DateTime.UtcNow;
            _logger.LogInformation($"Expiry thread invoked at {currentTime}");

            var res = _expiryQueue.Dequeue();
            if (res.IsFailure) return;
            var queueHead = (PriorityQueueItem)res.Value;
            do {
                _lruCache.TryRemove(queueHead.Key);
                _logger.LogInformation($"Removed Key:{queueHead.Key} that expired at {queueHead.Key}");

                res = _expiryQueue.Dequeue();
                if (res.IsFailure) return;
                queueHead = (PriorityQueueItem)res.Value;
            } while (currentTime >= res.Value.Priority);
        }
        public int Count => this._lruCache.Count;
    }
}