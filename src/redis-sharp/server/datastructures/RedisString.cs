namespace redis_sharp.server.datastructures
{
    public class RedisString : RedisObject
    {
        private readonly string value;

        public RedisString(string value)
        {
            this.value = value;
        }

        public override bool IsRedisString()
        {
            return true;
        }

        public override string ToString()
        {
            return value;
        }
    }
}