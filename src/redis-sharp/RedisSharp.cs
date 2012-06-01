using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using redis_sharp.server;


namespace redis_sharp
{
    public class RedisSharp
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public RedisSharp()
        {
        }

        public static void StartListening()
        {
            var ipAddress = IPAddress.Loopback;
            var localEndPoint = new IPEndPoint(ipAddress, 2046);
            var listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(AcceptCallback,listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            // Create the state object.
            var redisRequest = new RedisRequest(handler);
            handler.BeginReceive(redisRequest.Buffer, 0,redisRequest.NumberOfBytesToRead , 0,ReadCallback, redisRequest);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            Console.WriteLine("reading ...");

            var redisRequest = (RedisRequest)ar.AsyncState;
            var clientSocket = redisRequest.ClientSocket;

            // Read data from the client socket. 
            int bytesRead = clientSocket.EndReceive(ar);

            Console.WriteLine("read bytes --> "+bytesRead);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                var value = Encoding.ASCII.GetString(redisRequest.Buffer, 0, bytesRead);
                Console.WriteLine("read value --> " + value);
                redisRequest.AddData(value);

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                if (redisRequest.IsComplete())
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Command --> " + redisRequest);
                    // Echo the data back to the client.
                    Send(clientSocket, "+PONG\r\n");
                }
                else
                {
                    // Not all data received. Get more.
                    clientSocket.BeginReceive(redisRequest.Buffer, 0, redisRequest.NumberOfBytesToRead, 0,ReadCallback, redisRequest);
                }
            }
        }

        private static void Send(Socket clientSocket, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            var byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            clientSocket.BeginSend(byteData, 0, byteData.Length, 0,SendCallback, clientSocket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                var handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                var bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

//                handler.Shutdown(SocketShutdown.Both);
//                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}
