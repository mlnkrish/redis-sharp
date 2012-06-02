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
            stopwatch.Start();

            var redisClient = new RedisClient();;
            for (int i = 0; i < 100000; i++)
            {
                redisClient.Ping();
            }

            stopwatch.Stop();
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
                    list[j].Ping();
                }
            }

            stopwatch.Stop();
            Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void ThreadedConnections()
        {
            var threads = new List<Thread>();
            for (int i = 0; i < 7; i++)
            {
//                var list = new List<RedisClient>();
//                for (int j = 0; j < 50; j++)
//                {
//                    list.Add(new RedisClient(););
//                }
                var client = new RedisClient();;
                threads.Add(new Thread(() =>
                               {
                                   var stopwatch = new Stopwatch();
                                   stopwatch.Start();
                                   long responeTime = 0;

                                   for (int k = 0; k < 100000; k++)
                                   {
//                                       for (int j = 0; j < list.Count; j++)
//                                       {
//                                           list[j].Ping();
//                                       }
                                       var stopwatchq = new Stopwatch();
                                       stopwatchq.Start();
                                       client.Ping();
                                       stopwatchq.Stop();
                                       responeTime += stopwatchq.ElapsedMilliseconds;
                                   }

                                   stopwatch.Stop();
                                   Console.WriteLine("---->" + stopwatch.ElapsedMilliseconds);
                                   Console.WriteLine("X---->" + responeTime/100000);
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