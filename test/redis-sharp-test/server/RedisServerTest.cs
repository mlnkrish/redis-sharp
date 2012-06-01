
using NUnit.Framework;
using ServiceStack.Redis;

namespace redis_sharp_test.server
{
    public class RedisServerTest
    {

        [Test]
        public void testSomething()
        {
            var redisClient = new RedisClient("127.0.0.1",2046);
            bool ping = redisClient.Ping();
            Assert.IsTrue(ping);
        }
    }
}