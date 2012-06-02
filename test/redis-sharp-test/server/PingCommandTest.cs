using NUnit.Framework;
using ServiceStack.Redis;

namespace redis_sharp_test.server
{
    [TestFixture]
    public class PingCommandTest
    {
        [Test]
        public void GetPongForPing()
        {
            var redisClient = new RedisClient();

            Assert.That(redisClient.Ping(), Is.True);
        }
    }
}