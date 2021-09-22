using System;
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
                case CommandConstants.COMMAND_GET_GAMES_CODE:
                    finalCommand = new GetGamesCommand();
                    break;
                case CommandConstants.COMMAND_SEARCH_GAMES_CODE:
                    finalCommand = new SearchGamesCommand();
                    break;
                case CommandConstants.COMMAND_EXIT_CODE:
                    finalCommand = new ExitCommand();
                    break;
                case CommandConstants.COMMAND_PUBLISH_GAME_CODE:
                    finalCommand = new PublishGameCommand();
                    break;
                case CommandConstants.COMMAND_SELECT_GAME_CODE:
                    finalCommand = new SelectGameCommand();
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