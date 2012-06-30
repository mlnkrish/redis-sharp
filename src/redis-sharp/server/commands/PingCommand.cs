using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    internal class PingCommand : RedisCommand
    {
        public override bool Validate(Request request)
        {
            return true;
        }

        public override string DoProcess(Request request)
        {
            return Reply.Pong();
        }
    }
}