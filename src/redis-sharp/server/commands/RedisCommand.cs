using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    public abstract class RedisCommand : ICommand
    {
        public abstract bool Validate(Request request);
        public abstract string DoProcess(Request request);
        public string Process(Request request)
        {
            if(!Validate(request))
            {
                return Reply.ErrBadRequest();
            }
            return DoProcess(request);
        }
    }
}