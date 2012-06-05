using System.Collections.Generic;
using redis_sharp.server.daemons;
using redis_sharp.server.datastructures;
using redis_sharp.server.queues;

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
                                                                          {"GET", new GetCommand(Store)},
                                                                          {"APPEND", new AppendCommand(Store)},
                                                                          {"STRLEN", new StrlenCommand(Store)}
                                                                          /*Strings section END*/

                                                                      };

        public static string ProcessRequest(Request request)
        {
            if(!CommandTable.ContainsKey(request.command))
                return Reply.ErrInvalidCommand(request.command);
            return CommandTable[request.command].Process(request);
        }
    }
}