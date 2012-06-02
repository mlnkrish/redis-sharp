using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                redisClient.Ping();
            }
            stopwatch.Stop();
            Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
        }
    }
}