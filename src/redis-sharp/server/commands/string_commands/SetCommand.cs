using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands.string_commands
{
    internal class SetCommand : RedisCommand
    {
        private readonly KeyValueStore store;

        public SetCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public override bool Validate(Request request)
        {
            return request.args.Count == 2;
        }

        public override string DoProcess(Request request)
        {
            store.Set(request.args[0],new RedisString(request.args[1]));
            return Reply.Ok();
        }
    }
}