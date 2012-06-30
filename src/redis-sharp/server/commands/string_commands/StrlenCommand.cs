using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class StrlenCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public StrlenCommand(KeyValueStore store)
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
            if(redisObject == null)
            {
                return Reply.IntgerReply(0);
            }

            if(!redisObject.IsRedisString())
            {
                return Reply.ErrWrongType();
            }

            return Reply.IntgerReply((redisObject as RedisString).Length());
        }
    }
}