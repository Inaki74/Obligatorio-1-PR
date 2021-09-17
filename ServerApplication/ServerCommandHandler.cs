using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;

namespace ServerApplication
{
    public class ServerCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameter

        private readonly INetworkStreamHandler _networkStreamHandler;
        
        public ServerCommandHandler(INetworkStreamHandler streamHandler)
        {
            _networkStreamHandler = streamHandler;
        }
        
        public void ExecuteCommand(int command, IPayload payload)
        {
            ICommand finalCommand = new LoginCommand(_networkStreamHandler);

            switch (command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand(_networkStreamHandler);
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    // tirar excepcion
                    break;
                
            }
        }
    }
}

