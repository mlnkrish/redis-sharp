using NUnit.Framework;
using ServiceStack.Redis;

namespace redis_sharp_test.server.commands
{
    [TestFixture]
    public class StringCommandsTest
    {
        [Test]
        public void ShouldBeAbleToGetAndSetStrings()
        {
            var redisClient = new RedisClient();

            bool response = redisClient.Set("foo", "bar");
            Assert.That(response, Is.True);

            var val = redisClient.Get<string>("foo");
            Assert.That(val, Is.EqualTo("bar"));
        }

        [Test]
        public void ShouldGetNullWhenTryingToAccessANonExistingKey()
        {
            var redisClient = new RedisClient();
            var val = redisClient.Get<string>("nokey");

            Assert.That(val, Is.Null);
        }
    }
}