﻿using System;
using NUnit.Framework;
using ServiceStack.Redis;

namespace redis_sharp_test.server.commands
{
    [TestFixture]
    public class StringCommandsTest
    {
        RedisClient redisClient = new RedisClient();

        [Test]
        public void ShouldBeAbleToGetAndSetStrings()
        {
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key,"bar");
            redisClient.SetEntry(key, "bar");

            var val = redisClient.Get<string>(key);
            Assert.That(val, Is.EqualTo("bar"));
        }

        [Test]
        public void ShouldGetNullWhenTryingToAccessANonExistingKey()
        {
            var nokey = Guid.NewGuid().ToString();
            var val = redisClient.Get<string>(nokey);

            Assert.That(val, Is.Null);
        }

        [Test]
        public void ShouldAppendToAndExisitngString()
        {
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
            var key = Guid.NewGuid().ToString();
            var length = redisClient.AppendToValue(key, "baz");
            Assert.That(length,Is.EqualTo(3));

            var val = redisClient.Get<string>(key);

            Assert.That(val, Is.EqualTo("baz"));
        }

        [Test]
        public void ShouldGetLengthOfStringStoredInTheKey()
        {
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key, "1234567");


            var val = redisClient.StrLen(key);

            Assert.That(val, Is.EqualTo(7));
        }

        [Test]
        public void ShouldGetZeroAsLengthOfStringWhenTheKeyIsNotSet()
        {
            var key = Guid.NewGuid().ToString();

            var val = redisClient.StrLen(key);

            Assert.That(val, Is.EqualTo(0));
        }

        [Test]
        public void ShouldIncrementValueByOne()
        {
            var key1 = Guid.NewGuid().ToString();
            redisClient.SetEntry(key1, "10");
            var key2 = Guid.NewGuid().ToString();
            redisClient.SetEntry(key2, "-10");

            var val1 = redisClient.Incr(key1);
            Assert.That(val1, Is.EqualTo(11));
            var val2 = redisClient.Incr(key2);
            Assert.That(val2, Is.EqualTo(-9));
        }

        [Test]
        public void ShouldCreateKeyAndSetItsValueToZeroBeforeIncrementing()
        {
            var key = Guid.NewGuid().ToString();
            
            var val1 = redisClient.Incr(key);
            Assert.That(val1, Is.EqualTo(1));

            var strValue = redisClient.Get<String>(key);
            Assert.That(strValue, Is.EqualTo("1"));
        }

        [Test]
        [ExpectedException(typeof(RedisResponseException))]
        public void ShouldErrorOnWhenKeyHasANonLongValue_Incr()
        {
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key, "abcd");

            redisClient.Incr(key);
        }

        [Test]
        public void ShouldDecrementValueByOne()
        {
            var key1 = Guid.NewGuid().ToString();
            redisClient.SetEntry(key1, "10");
            var key2 = Guid.NewGuid().ToString();
            redisClient.SetEntry(key2, "-10");

            var val1 = redisClient.Decr(key1);
            Assert.That(val1, Is.EqualTo(9));
            var val2 = redisClient.Decr(key2);
            Assert.That(val2, Is.EqualTo(-11));
        }

        [Test]
        public void ShouldCreateKeyAndSetItsValueToZeroBeforeDecrementing()
        {
            var key = Guid.NewGuid().ToString();
            
            var val1 = redisClient.Decr(key);
            Assert.That(val1, Is.EqualTo(-1));

            var strValue = redisClient.Get<String>(key);
            Assert.That(strValue, Is.EqualTo("-1"));
        }

        [Test]
        [ExpectedException(typeof(RedisResponseException))]
        public void ShouldErrorOnWhenKeyHasANonLongValue_Decr()
        {
            var key = Guid.NewGuid().ToString();
            redisClient.SetEntry(key, "abcd");

            redisClient.Decr(key);
        }
    }
}