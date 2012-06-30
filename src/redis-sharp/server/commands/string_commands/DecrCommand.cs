using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class DecrCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public DecrCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public override bool Validate(Request request)
        {
            return request.args.Count == 1;
        }

        public override string DoProcess(Request request)
        {
            var redisObject = store.Get(request.args[0]);

            if (redisObject == null)
            {
                var redisString = new RedisString("0");
                long longValue;
                redisString.DecrementBy(1, out longValue);
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
                var success = s.DecrementBy(1, out longValue);
                return success ? Reply.IntgerReply(longValue) : Reply.OverFlow();
            }
            return Reply.ValueNotInt();
        }
    }
}