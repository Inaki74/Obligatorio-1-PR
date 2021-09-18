using System;
using ClientApplicationInterfaces;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;

namespace ClientApplication
{
    public class ClientCommandHandler : IClientCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameter
        
        public VaporStatusResponse ExecuteCommand(VaporProcessedPacket processedPacket)
        {
            ICommand finalCommand = new LoginCommand();

            switch (processedPacket.Command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand();
                    break;
                case CommandConstants.COMMAND_EXIT_CODE:
                    finalCommand = new ExitCommand();
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    // tirar excepcion
                    break;
                
            }
            
            return finalCommand.ActionRes(processedPacket.Payload);
        }
        
        

    }
}