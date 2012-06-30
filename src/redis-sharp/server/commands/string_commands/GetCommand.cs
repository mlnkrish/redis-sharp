using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class GetCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public GetCommand(KeyValueStore store)
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
                return Reply.Nil();
            if (!redisObject.IsRedisString())
                return Reply.ErrWrongType();
            return Reply.BulkReply(redisObject.ToString());
        }

    }
}