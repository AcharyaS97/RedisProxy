using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisProxy.Services {

    public interface ICacheConnection {
        private static string _configurationOptions;

        public string GetItem(string key);
    }
}