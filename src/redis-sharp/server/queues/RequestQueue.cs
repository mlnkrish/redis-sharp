using System.Collections.Concurrent;

namespace redis_sharp.server.queues
{
    public class RequestQueue
    {
        private readonly ConcurrentQueue<Request> queue = new ConcurrentQueue<Request>();

        public void Enqueue(Request request)
        {
            queue.Enqueue(request);
        }

        public Request Dequeue()
        {
            Request request;
            queue.TryDequeue(out request);
            return request;
        }
    }
}