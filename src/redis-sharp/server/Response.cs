namespace redis_sharp.server
{
    public class Response
    {
        public static string Ok()
        {
            return "+OK\r\n";
        }

        public static string Pong()
        {
            return "+PONG\r\n";
        }

        public static string BulkReply(string val)
        {
            return string.Format("${0}\r\n{1}\r\n",val.Length,val);
        }

        public static string Nil()
        {
            return "$-1\r\n";
        }

        public static string ErrWrongType()
        {
            return "-Operation against a key holding the wrong kind of value.\r\n";
        }
    }
}