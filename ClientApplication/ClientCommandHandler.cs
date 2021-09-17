using System;
using Common.Commands;
using Common.Interfaces;

namespace ClientApplication
{
    public class ClientCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameter
        

        
        public void ExecuteCommand(int command, IPayload payload)
        {
            switch (command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    ICommand loginCommand = new LoginCommand();
                    loginCommand.ActionReq(payload);
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    break;
                
            }
        }
        
        

    }
}