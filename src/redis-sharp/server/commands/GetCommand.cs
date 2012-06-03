using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    internal class GetCommand : ICommand
    {
        private readonly KeyValueStore store;

        public GetCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public string Process(Request request)
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