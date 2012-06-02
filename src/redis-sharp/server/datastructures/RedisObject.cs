namespace redis_sharp.server.datastructures
{
    public class RedisObject
    {
        public bool IsRedisObject()
        {
            return true;
        }

        public virtual bool IsRedisString()
        {
            return false;
        }
    }
}