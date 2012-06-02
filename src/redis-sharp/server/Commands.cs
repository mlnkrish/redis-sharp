using System.Collections.Generic;

namespace redis_sharp.server
{
    public class Commands
    {
        private static readonly Dictionary<string, ICommand> CommandTable= new Dictionary<string, ICommand>()
                                                                      {
                                                                          {"PING", new PingCommand()}
                                                                      };

        public static string ProcessRequest(Request request)
        {
            /*TODO guards*/
            return CommandTable[request.Command].Process(request);
        }
    }

    internal class PingCommand : ICommand
    {
        public string Process(Request request)
        {
            return "+PONG\r\n";
        }
    }

    internal interface ICommand
    {
        string Process(Request request);
    }
}