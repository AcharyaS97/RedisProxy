using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisProxy.Services {

    public interface ILocalCache {

        public bool GetKey(string key, out string returnValue);

        public void AddOrUpdateKey(string key, string value);
    }
}