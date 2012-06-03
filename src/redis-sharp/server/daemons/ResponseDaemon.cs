using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using redis_sharp.server.queues;

namespace redis_sharp.server.daemons
{
    public class ResponseDaemon
    {
        private readonly ResponseQueue responseQueue;
        private readonly int numberOfProcessors;
        private readonly List<Thread> processors = new List<Thread>();

        public ResponseDaemon(ResponseQueue responseQueue)
        {
            this.responseQueue = responseQueue;
            this.numberOfProcessors = 4;
        }

        public void Start()
        {
            for (int i = 0; i < numberOfProcessors; i++)
            {
                var thread = new Thread(SendResponseToClient);
                processors.Add(thread);
                thread.Start();
            }
        }

        private void SendResponseToClient()
        {
            while(true)
            {
                var response = responseQueue.Dequeue();
                if(response != null)
                {
//                    Console.WriteLine("Found Response. Sending..");
//                    Console.WriteLine("Client is active --> " + response.client.Poll(100,SelectMode.SelectWrite));
//                    Console.WriteLine("reply -->" + response.reply);
                    
                    Send(response.client,response.reply);           
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private static void Send(Socket clientSocket, String data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);
            clientSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, clientSocket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var clientSocket = (Socket)ar.AsyncState;
            clientSocket.EndSend(ar);
        }
    }
}