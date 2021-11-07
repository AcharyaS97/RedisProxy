using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using RedisProxy.Services;
using System.Collections.Generic;
using StackExchange.Redis;

namespace RedisProxy.Controllers {

    [Route("proxy/")]
    [ApiController]
    public class RedisProxyController : ControllerBase {
        private readonly ILocalCache _localCache;
        private readonly ICacheConnection _redisMgr;

        public RedisProxyController(ILocalCache dict, ICacheConnection redisConn) {
            _localCache = dict;
            _redisMgr = redisConn;
        }

        [HttpGet("{key}")]
        public string GetFromCache(string key) {
            var ret = _localCache.GetKey(key, out var cacheRet);

            if (!ret) {
                var dbValue = _redisMgr.GetItem(key);
                if (dbValue != null) {
                    _localCache.AddOrUpdateKey(key, dbValue);
                }
                cacheRet = dbValue;
            }
            return cacheRet;
        }
    }
}