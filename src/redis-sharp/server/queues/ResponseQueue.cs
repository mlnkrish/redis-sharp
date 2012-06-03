using System.Collections.Concurrent;

namespace redis_sharp.server.queues
{
    public class ResponseQueue
    {
        private readonly ConcurrentQueue<Response> queue = new ConcurrentQueue<Response>();

        public void Enqueue(Response response)
        {
            queue.Enqueue(response);
        }
 
        public Response Dequeue()
        {
            Response response;
            queue.TryDequeue(out response);
            return response;
        }
    }
}