namespace redis_sharp.server.datastructures
{
    public class RedisString : RedisObject
    {
        private string value;

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

        public int Length()
        {
            return value.Length;
        }

        public void Append(string s)
        {
            value = value + s;
        }
    }
}