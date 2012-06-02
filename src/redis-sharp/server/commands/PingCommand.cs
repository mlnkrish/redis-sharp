namespace redis_sharp.server.commands
{
    internal class PingCommand : ICommand
    {
        public string Process(Request request)
        {
            return Response.Pong();
        }
    }
}