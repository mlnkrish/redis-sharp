namespace redis_sharp.server.commands
{
    public class Reply
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

        public static string ErrInvalidCommand(string command)
        {
            return string.Format("-Unknown or disabled command '{0}'", command);
        }

        public static string IntgerReply(long val)
        {
            return string.Format(":{0}\r\n", val);
        }

        public static string OverFlow()
        {
            return string.Format("-Increment or Decrement will overflow\r\n");
        }

        public static string ValueNotInt()
        {
            return string.Format("-Value not integer or out of range\r\n");    
        }
    }
}