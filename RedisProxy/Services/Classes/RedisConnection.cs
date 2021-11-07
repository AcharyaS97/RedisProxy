using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisProxy.Services {

    public class RedisConnection : ICacheConnection {
        private static string _configurationOptions;

        public RedisConnection(string configurationOptions) {
            if (configurationOptions == null) throw new ArgumentNullException("configurationOptions");
            _configurationOptions = configurationOptions;
        }

        private static IDatabase Cache {
            get {
                return Connection.GetDatabase();
            }
        }

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection
          = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_configurationOptions));

        public static ConnectionMultiplexer Connection {
            get {
                return LazyConnection.Value;
            }
        }

        public string GetItem(string key) {
            var output = Cache.StringGet(key);
            if (output.HasValue) {
                return output.ToString();
            }
            return null;
        }
    }
}