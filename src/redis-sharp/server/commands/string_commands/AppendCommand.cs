using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class AppendCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public AppendCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public override bool Validate(Request request)
        {
            return request.args.Count == 2;
        }

        public override string DoProcess(Request request)
        {
            var redisObject = store.Get(request.args[0]);

            if(redisObject == null)
            {
                var redisString = new RedisString(request.args[1]);
                store.Set(request.args[0],redisString);
                return Reply.IntgerReply(redisString.Length());
            }

            if (!redisObject.IsRedisString())
            {
                return Reply.ErrWrongType();
            }

            var s = redisObject as RedisString;
            s.Append(request.args[1]);
            return Reply.IntgerReply(s.Length());
        }
    }
}