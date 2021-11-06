using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisProxy.Properties {

    public static class StartupArgs {
        public const string key_HostName = "HOST_NAME";
        public const string key_MaxSize = "MAX_CACHE_SIZE";
        public const string key_GlobalExpiry = "GLOBAL_EXPIRY_TIME";
    }
}