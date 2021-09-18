using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;

namespace ClientApplication
{
    public class ClientCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameter
        
        public void ExecuteCommand(VaporProcessedPacket processedPacket)
        {
            ICommand finalCommand = new LoginCommand();

            switch (processedPacket.Command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand();
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    // tirar excepcion
                    break;
                
            }
            
            finalCommand.ActionRes(processedPacket.Payload);
        }
        
        

    }
}