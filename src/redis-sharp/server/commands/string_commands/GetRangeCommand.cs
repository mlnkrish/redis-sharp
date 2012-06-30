using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class GetRangeCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public GetRangeCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public override bool Validate(Request request)
        {
            if(request.args.Count == 3)
            {
                int i;
                return int.TryParse(request.args[1], out i) && int.TryParse(request.args[2], out i);
            }
            return false;
        }

        public override string DoProcess(Request request)
        {
            var redisObject = store.Get(request.args[0]);
            if(redisObject == null)
            {
                return Reply.BulkReply("");
            }

            if(!redisObject.IsRedisString())
            {
                return Reply.ErrWrongType();
            }

            return Reply.BulkReply((redisObject as RedisString).Substring(int.Parse(request.args[1]),int.Parse(request.args[2])));
        }
    }
}