using System;
using System.Threading;
using redis_sharp.server.commands;
using redis_sharp.server.queues;

namespace redis_sharp.server.daemons
{
    public class CommandDaemon
    {
        private readonly RequestQueue requestQueue;
        private readonly ResponseQueue responseQueue;
        private Thread handlerThread;

        public CommandDaemon(RequestQueue requestQueue, ResponseQueue responseQueue)
        {
            this.requestQueue = requestQueue;
            this.responseQueue = responseQueue;
        }

        public void Start()
        {
            handlerThread = new Thread(HandleRequests);
            handlerThread.Start();
        }

        private void HandleRequests()
        {
            while(true)
            {
                var request = requestQueue.Dequeue();
                if(request != null)
                {
//                    Console.WriteLine("Found Request Processing..");
//                    Console.WriteLine("command --> " + request.command);

                    var response = Commands.ProcessRequest(request);
                    responseQueue.Enqueue(new Response()
                                              {
                                                  client = request.client,
                                                  reply = response
                                              });

//                    Console.WriteLine("Added Response to queue");
//                    Console.WriteLine("response --> " + response);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}