using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class IncByCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public IncByCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public override bool Validate(Request request)
        {
            if (request.args.Count == 2)
            {
                long res;
                return long.TryParse(request.args[1], out res);
            }
            return false;
        }

        public override string DoProcess(Request request)
        {
            var redisObject = store.Get(request.args[0]);
            var valToIncBy = long.Parse(request.args[1]);
            if (redisObject == null)
            {
                var redisString = new RedisString("0");
                long longValue;
                redisString.IncrementBy(valToIncBy, out longValue);
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
                var success = s.IncrementBy(valToIncBy, out longValue);
                return success ? Reply.IntgerReply(longValue) : Reply.OverFlow();
            }
            return Reply.ValueNotInt();
        }
    }
}