using System;
using System.Collections.Generic;
using RedisProxy.Controllers;
using Xunit;

namespace ProxyTests {

    public class RedisProxyControllerTest {
        private readonly RedisProxyController _controller;

        public RedisProxyControllerTest() {
            var testCache = SetupLocalTestData();
            _controller = new RedisProxyController(testCache);
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

        private Dictionary<string, string> SetupLocalTestData() {
            var cache = new Dictionary<string, string>();
            cache.Add("Key1", "hello");
            cache.Add("Key2", "hello2");
            return cache;
        }
    }
}