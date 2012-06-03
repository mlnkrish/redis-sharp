using System;
using redis_sharp.server.daemons;
using redis_sharp.server.queues;

namespace redis_sharp
{
    public class RedisSharp
    {
        private static ResponseDaemon responseDaemon;
        private static ServerDaemon serverDaemon;
        private static CommandDaemon commandDaemon;

        public static int Main(String[] args)
        {
            var requestQueue = new RequestQueue();
            var responseQueue = new ResponseQueue();
            responseDaemon = new ResponseDaemon(responseQueue);
            serverDaemon = new ServerDaemon(requestQueue);
            commandDaemon = new CommandDaemon(requestQueue,responseQueue);

            responseDaemon.Start();
            commandDaemon.Start();
            serverDaemon.Start();
            Console.ReadKey();
            return 0;
        }
    }
}