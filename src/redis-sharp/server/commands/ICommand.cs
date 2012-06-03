using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    internal interface ICommand
    {
        string Process(Request request);
    }
}