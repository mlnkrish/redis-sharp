using System.Collections.Generic;
using redis_sharp.server.datastructures;

namespace redis_sharp.server.commands
{
    public class Commands
    {
        private static readonly KeyValueStore Store = new KeyValueStore();

        /*Need not be concurrent dictionary, all operation on this are gets which are thread safe*/
        private static readonly Dictionary<string, ICommand> CommandTable= new Dictionary<string, ICommand>()
                                                                      {
                                                                          {"PING", new PingCommand()},

                                                                          /*Strings section START*/
                                                                          {"SET", new SetCommand(Store)},
                                                                          {"GET", new GetCommand(Store)}
                                                                          /*Strings section END*/

                                                                      };

        public static string ProcessRequest(Request request)
        {
            /*TODO guards*/
            return CommandTable[request.Command].Process(request);
        }
    }
}