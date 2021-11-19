using System;

namespace Common.Commands
{
    public struct CommandResponse
    {
        public readonly string Response {get;}
        public readonly string Command {get;}

        public CommandResponse(string response, string command)
        {
            Response = response;
            Command = command;
        }
    }
}
