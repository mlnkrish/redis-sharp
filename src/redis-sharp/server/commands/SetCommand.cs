using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

namespace redis_sharp.server.commands
{
    internal class SetCommand : ICommand
    {
        private readonly KeyValueStore store;

        public SetCommand(KeyValueStore store)
        {
            this.store = store;
        }

        public string Process(Request request)
        {
            store.Set(request.args[0],new RedisString(request.args[1]));
            return Reply.Ok();
        }
    }
}