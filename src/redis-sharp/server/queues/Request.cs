using System.Collections.Generic;
using System.Net.Sockets;

namespace redis_sharp.server.queues
{
    public class Request
    {
        public Socket client;
        public string command;
        public List<string> args;

        public override string ToString()
        {
            return string.Format("Client: {0}, Command: {1}, Args: {2}", client, command, args);
        }
    }
}