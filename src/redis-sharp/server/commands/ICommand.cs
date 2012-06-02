using redis_sharp.server;

namespace redis_sharp.server.commands
{
    internal interface ICommand
    {
        string Process(Request request);
    }
}