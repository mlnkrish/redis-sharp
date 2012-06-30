using System;
using System.Globalization;

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

        public bool IsConvertibleToLong()
        {
            long res;
            return long.TryParse(value, out res);
        }

        public bool DecrementBy(long val, out long longValue)
        {
            var l = long.Parse(value);
            if (l>=0)
            {
                if (val >= ((long.MaxValue - l)*-1))
                {
                    longValue = l - val;
                    value = longValue.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            else
            {
                if (val <= (l-long.MinValue))
                {
                    longValue = l - val;
                    value = longValue.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            longValue = 0;
            return false;
        }

        public bool IncrementBy(long val, out long longValue)
        {
            var l = long.Parse(value);
            if (l >= 0)
            {
                if (val <= (long.MaxValue - l))
                {
                    longValue = l + val;
                    value = longValue.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            else
            {
                if (val >= ((l - long.MinValue)*-1))
                {
                    longValue = l + val;
                    value = longValue.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            longValue = 0;
            return false;
        }
    }
}