using System;
using System.Collections.Generic;
using RedisProxy.Controllers;
using RedisProxy.Services;
using Xunit;

namespace ProxyTests {

    public class RedisProxyControllerTest {
        private readonly RedisProxyController _controller;

        private readonly RedisConnection _connection;

        private readonly LRUCache _localCache;

        public RedisProxyControllerTest() {
            _connection = new RedisConnection(GetConnectionString());
            _localCache = new LRUCache(100);
            SetupLocalTestData();

            _controller = new RedisProxyController(_localCache, _connection);
        }

        [Fact]
        public void TestLocalCacheAccess_KeyExists() {
            var getResult = _controller.GetFromCache("Key1");
            Assert.NotNull(getResult);
        }

        [Fact]
        public void TestLocalCacheAccess_KeyNotExists() {
            var getResult = _controller.GetFromCache("KeyNotReal");
            Assert.Null(getResult);
        }

        private void SetupLocalTestData() {
            _localCache.AddOrUpdateKey("Key1", "Val1");
            _localCache.AddOrUpdateKey("Key2", "Val2");
        }

        private string GetConnectionString() {
            return "rescale-test-saheel.redis.cache.windows.net:6380,password=E1PdLmEqmQ4BNPktVwfF5zd3Cv0fxzLq1AzCaLPCEPo=,ssl=True,abortConnect=False";
        }
    }
}