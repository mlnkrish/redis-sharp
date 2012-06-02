using redis_sharp.server.datastructures;

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
            var redisObject = store.Get(request.Args[0]);
            if (redisObject == null)
                return Response.Nil();
            if (!redisObject.IsRedisString())
                return Response.ErrWrongType();
            return Response.BulkReply(redisObject.ToString());
        }

    }
}