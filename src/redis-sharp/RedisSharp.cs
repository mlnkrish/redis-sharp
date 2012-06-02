using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using redis_sharp.server;
using redis_sharp.server.commands;


namespace redis_sharp
{
    public class RedisSharp
    {
        private static readonly ManualResetEvent ReadyForNextConnection = new ManualResetEvent(false);

        public static void StartListening()
        {
            var ipAddress = IPAddress.Loopback;
            var localEndPoint = new IPEndPoint(ipAddress, 6379);
            var listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    ReadyForNextConnection.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(OnAcceptConnection,listener);

                    ReadyForNextConnection.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void OnAcceptConnection(IAsyncResult ar)
        {
            ReadyForNextConnection.Set();

            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            var redisRequest = new Request(handler);
            handler.BeginReceive(redisRequest.Buffer, 0,redisRequest.NumberOfBytesToRead , 0,ReadCallback, redisRequest);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            var redisRequest = (Request)ar.AsyncState;
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
                    Send(clientSocket, Commands.ProcessRequest(redisRequest));
                }
                else
                {
                    clientSocket.BeginReceive(redisRequest.Buffer, 0, redisRequest.NumberOfBytesToRead, 0,ReadCallback, redisRequest);
                }
            }
        }

        private static void Send(Socket clientSocket, String data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);
            clientSocket.BeginSend(byteData, 0, byteData.Length, 0,SendCallback, clientSocket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var clientSocket = (Socket)ar.AsyncState;

                clientSocket.EndSend(ar);

                var redisRequest = new Request(clientSocket);
                clientSocket.BeginReceive(redisRequest.Buffer, 0, redisRequest.NumberOfBytesToRead, 0, ReadCallback, redisRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        public static int Main(String[] args)
        {
            try
            {
                StartListening();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            return 0;
        }
    }
}
