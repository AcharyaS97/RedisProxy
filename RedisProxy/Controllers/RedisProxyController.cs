using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisProxy.Controllers {

    public class RedisProxyController {
        private readonly ILogger<RedisProxyController> _logger;

        private readonly Dictionary<string, object> _localCache;

        public RedisProxyController(ILogger<RedisProxyController> logger) {
            _logger = logger;
            _localCache = new Dictionary<string, object>();
        }

        [HttpGet]
        public string Get(string key) {
            throw new NotImplementedException();
        }
    }
}
}