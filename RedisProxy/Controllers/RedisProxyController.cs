using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using RedisProxy.Services;
using System.Collections.Generic;

namespace RedisProxy.Controllers {

    [Route("proxy/")]
    [ApiController]
    public class RedisProxyController : ControllerBase {
        private readonly ILocalCache _localCache;

        public RedisProxyController(ILocalCache dict) {
            _localCache = dict;
        }

        [HttpGet("{key}")]
        public string GetFromCache(string key) {
            var ret = _localCache.GetKey(key, out var cacheRet);
            if (!ret) {
                return null;
            }
            return cacheRet;
        }
    }
}