using System;
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
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key,"bar");
            redisClient.SetEntry(key, "bar");

            var val = redisClient.Get<string>(key);
            Assert.That(val, Is.EqualTo("bar"));
        }

        [Test]
        public void ShouldGetNullWhenTryingToAccessANonExistingKey()
        {
            var redisClient = new RedisClient();
            var nokey = Guid.NewGuid().ToString();
            var val = redisClient.Get<string>(nokey);

            Assert.That(val, Is.Null);
        }

        [Test]
        public void ShouldAppendToAndExisitngString()
        {
            var redisClient = new RedisClient();
            var key = Guid.NewGuid().ToString();

            redisClient.SetEntry(key, "bar");
            var length = redisClient.AppendToValue(key, "baz");
            Assert.That(length, Is.EqualTo(6));

            var val = redisClient.Get<string>(key);

            Assert.That(val, Is.EqualTo("barbaz"));
        }

        [Test]
        public void ShouldCreateNewIfStringDoesntExists()
        {
            var redisClient = new RedisClient();
            var key = Guid.NewGuid().ToString();
            var length = redisClient.AppendToValue(key, "baz");
            Assert.That(length,Is.EqualTo(3));

            var val = redisClient.Get<string>(key);

            Assert.That(val, Is.EqualTo("baz"));
        }

        [Test]
        public void ShouldGetLengthOfStringStoredInTheKey()
        {
            var redisClient = new RedisClient();
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key, "1234567");


            var val = redisClient.StrLen(key);

            Assert.That(val, Is.EqualTo(7));
        }

        [Test]
        public void ShouldGetZeroAsLengthOfStringWhenTheKeyIsNotSet()
        {
            var redisClient = new RedisClient();
            var key = Guid.NewGuid().ToString();

            var val = redisClient.StrLen(key);

            Assert.That(val, Is.EqualTo(0));
        }
    }
}