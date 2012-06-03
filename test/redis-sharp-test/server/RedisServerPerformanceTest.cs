using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using ServiceStack.Redis;

namespace redis_sharp_test.server
{
    [TestFixture]
    [Ignore]
    public class RedisServerPerformanceTest
    {
        [Test]
        public void SingleConnectionLooping()
        {
            var stopwatch = new Stopwatch();

            var redisClient = new RedisClient();;
            for (int i = 0; i < 10000; i++)
            {
//                redisClient.Ping();
                stopwatch.Restart();
                redisClient.Set(i.ToString(), "bar");
                stopwatch.Stop();
                Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
            }
            
            Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
        }


        [Test]
        public void MultipleConnectionLooping()
        {
            var list = new List<RedisClient>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new RedisClient());
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();


            for (int i = 0; i < 2000; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
//                    list[j].Ping();
                    list[j].Set(i.ToString() + j.ToString(), "bar");
                }
            }

            stopwatch.Stop();
            Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void ThreadedConnections()
        {
            var threads = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
//                var list = new List<RedisClient>();
//                for (int j = 0; j < 50; j++)
//                {
//                    list.Add(new RedisClient(););
//                }
                var client = new RedisClient();
                threads.Add(new Thread(() =>
                               {
                                   long responeTime = 0;

                                   for (int k = 0; k < 1000; k++)
                                   {
//                                       for (int j = 0; j < list.Count; j++)
//                                       {
//                                           list[j].Ping();
//                                       }
                                       var newGuid = Guid.NewGuid().ToString();
                                       var stopwatchq = new Stopwatch();
                                       stopwatchq.Restart();
                                       client.Set("foo","bar");
                                       stopwatchq.Stop();
                                       Console.WriteLine("---->" + stopwatchq.ElapsedMilliseconds);
                                       responeTime += stopwatchq.ElapsedMilliseconds;
                                   }

                                   Console.WriteLine("X---->" + responeTime);
                               }));
            }

            var stopwatchx = new Stopwatch();
            stopwatchx.Start();

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            stopwatchx.Stop();
            Console.WriteLine("TOTAL ---->" + stopwatchx.ElapsedMilliseconds);
        }
    }
}