﻿using System;
using Common.Interfaces;

namespace Common.Commands
{
    public abstract class CommandHandler
    {
        protected virtual ICommand DecideCommand(string command)
        {
            ICommand finalCommand = null;
            switch (command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand();
                    break;
                case CommandConstants.COMMAND_EXIT_CODE:
                    finalCommand = new ExitCommand();
                    break;
                case CommandConstants.COMMAND_PUBLISH_GAME_CODE:
                    finalCommand = new PublishGameCommand();
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    // tirar excepcion
                    break;
                
            }

            return finalCommand;
        }
    }
}