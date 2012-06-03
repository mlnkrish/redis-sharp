using System.Net.Sockets;

namespace redis_sharp.server.queues
{
    public class Response
    {
        public string reply;
        public Socket client;
    }
}