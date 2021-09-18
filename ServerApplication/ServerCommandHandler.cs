using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;

namespace ServerApplication
{
    public class ServerCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameterxw
        public CommandResponse ExecuteCommand(VaporProcessedPacket packet)
        {
            ICommand finalCommand = new LoginCommand();

            switch (packet.Command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand();
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    // tirar excepcion
                    break;
                
            }

            string response = finalCommand.ActionReq(packet.Payload);

            CommandResponse commandResponse = new CommandResponse(response, finalCommand.Command);
            
            return commandResponse;
        }
    }
}

