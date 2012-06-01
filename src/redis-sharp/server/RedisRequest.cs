using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace redis_sharp.server
{
    public enum RequestState
    {
        Uninitialized,
        ReadingCommandLength,
        ReadingCommandText,
        Complete
    }

    public class RedisRequest
    {
        private RequestState currentState = RequestState.Uninitialized;
        private int numberOfCommandsRead;
        private string rawUnprocesedCommand = "";
        private int currentCommandLength;

        public RedisRequest(Socket client)
        {
            Args = new List<string>();
            NumberOfBytesToRead = 4;
            ClientSocket = client;
            Buffer = new byte[NumberOfBytesToRead];
        }

        public byte[] Buffer { get; private set; }

        public Socket ClientSocket { get; private set; }

        public int NumberOfCommands { get; private set; }

        public List<string> Args { get; private set; }

        public string Command { get; private set; }

        public int NumberOfBytesToRead { get; private set; }

        public bool IsComplete()
        {
            return currentState.Equals(RequestState.Complete);
        }

        public void AddData(string data)
        {
            rawUnprocesedCommand = rawUnprocesedCommand + data;
            var strings = Regex.Split(rawUnprocesedCommand,"\r\n");
            for (int i = 0; i < strings.Length; i++)
            {
                var line = strings[i];
                if (i+1 == strings.Length)
                {
                    rawUnprocesedCommand = (line != "\r\n") ? line : "";

                    if (currentState.Equals(RequestState.ReadingCommandText))
                    {
                        NumberOfBytesToRead = currentCommandLength - rawUnprocesedCommand.Length + 2;
                    }
                    continue;
                }

                if (currentState.Equals(RequestState.Uninitialized))
                {
                    ReadTotalNumberOfCommands(line);
                }else if(currentState.Equals(RequestState.ReadingCommandLength))
                {
                    ReadCurrentCommandLength(line);
                }else if(currentState.Equals(RequestState.ReadingCommandText))
                {
                    ReadCommandText(line);
                }
            }
            if(!IsComplete())
            {
                Buffer = new byte[NumberOfBytesToRead];
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Command + " ");
            foreach (var arg in Args)
            {
                stringBuilder.Append(arg + " ");
            }
            return stringBuilder.ToString();
        }

        private void ReadCommandText(string line)
        {
            if(Command == null)
            {
                Command = line;
            }else
            {
                Args.Add(line);
            }

            numberOfCommandsRead++;
            if(numberOfCommandsRead == NumberOfCommands)
            {
                currentState = RequestState.Complete;
            }else
            {
                currentState = RequestState.ReadingCommandLength;
                NumberOfBytesToRead = 4;
            }
        }

        private void ReadCurrentCommandLength(string line)
        {
            currentCommandLength = int.Parse(line.Substring(1));
            currentState = RequestState.ReadingCommandText;
        }

        private void ReadTotalNumberOfCommands(string firstLine)
        {
            NumberOfCommands = int.Parse(firstLine.Substring(1));
            /*TODO Handle if number of commands is 0*/
            currentState = RequestState.ReadingCommandLength;
        }
    }
}