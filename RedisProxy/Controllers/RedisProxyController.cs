using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace RedisProxy.Controllers {

    [Route("proxy/")]
    [ApiController]
    public class RedisProxyController : ControllerBase {
        private readonly Dictionary<string, string> _localCache;

        public RedisProxyController(Dictionary<string, string> dict) {
            _localCache = dict;
        }

        [HttpGet("{key}")]
        public string GetFromCache(string key) {
            var output = _localCache.TryGetValue(key, out var cacheRet);

            if (!output) {
                return null;
            }
            return cacheRet;
        }

        [HttpGet]
        public string Get(string key) {
            throw new NotImplementedException();
        }
    }
}