using redis_sharp.server.datastructures;

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
            store.Set(request.Args[0],new RedisString(request.Args[1]));
            return Response.Ok();
        }
    }
}