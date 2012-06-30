using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using redis_sharp.server.queues;

namespace redis_sharp.server.daemons
{
    public class ServerDaemon
    {
        private readonly RequestQueue requestQueue;
        private static readonly ManualResetEvent ReadyForNextConnection = new ManualResetEvent(false);

        public ServerDaemon(RequestQueue requestQueue)
        {
            this.requestQueue = requestQueue;
        }

        private void StartListening()
        {
            var ipAddress = IPAddress.Loopback;
            var localEndPoint = new IPEndPoint(ipAddress, 6379);
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

         
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                ReadyForNextConnection.Reset();

                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(OnAcceptConnection, listener);

                ReadyForNextConnection.WaitOne();
            }
         
        }

        private void OnAcceptConnection(IAsyncResult ar)
        {
            ReadyForNextConnection.Set();

            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            var redisRequest = new ClientRequest(handler);
            handler.BeginReceive(redisRequest.Buffer, 0, redisRequest.NumberOfBytesToRead, 0, ReadCallback, redisRequest);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var redisRequest = (ClientRequest)ar.AsyncState;
            var clientSocket = redisRequest.ClientSocket;
            var bytesRead = 0;
            try
            {
                bytesRead = clientSocket.EndReceive(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            if (bytesRead > 0)
            {
                var value = Encoding.ASCII.GetString(redisRequest.Buffer, 0, bytesRead);
                redisRequest.AddData(value);

                if (redisRequest.IsComplete())
                {
                    requestQueue.Enqueue(new Request
                                             {
                                                 args = redisRequest.Args,
                                                 command = redisRequest.Command,
                                                 client = redisRequest.ClientSocket
                                             });

//                    Console.WriteLine("Added request to queue");
//                    Console.WriteLine("request -->" + redisRequest.Command);

                    redisRequest.Reset();
                }
                clientSocket.BeginReceive(redisRequest.Buffer, 0, redisRequest.NumberOfBytesToRead, 0, ReadCallback, redisRequest);
            }
        }


        public void Start()
        {
            try
            {
                StartListening();
            }
            catch (Exception e)
            {
                Console.WriteLine("The server died :(");
                Console.WriteLine(e);
            }
        }
    }
}