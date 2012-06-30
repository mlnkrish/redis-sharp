using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    internal class IncrCommand : ICommand
    {
        private readonly KeyValueStore store;

        public IncrCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public string Process(Request request)
        {
            var redisObject = store.Get(request.args[0]);

            if (redisObject == null)
            {
                var redisString = new RedisString("0");
                long longValue;
                redisString.IncrementBy(1, out longValue);
                store.Set(request.args[0], redisString);
                return Reply.IntgerReply(longValue);
            }
            if (!redisObject.IsRedisString())
            {
                return Reply.ErrWrongType();
            }

            var s = redisObject as RedisString;
            if(s.IsConvertibleToLong())
            {
                long longValue;
                var success = s.IncrementBy(1, out longValue);
                return success ? Reply.IntgerReply(longValue) : Reply.OverFlow();
            }
            return Reply.ValueNotInt();
        }
    }
}