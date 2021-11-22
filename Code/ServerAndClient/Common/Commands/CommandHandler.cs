using System;
using Common.Interfaces;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public abstract class CommandHandler
    {
        protected virtual ICommand DecideCommand(string command, ILogSender logSender)
        {
            ICommand finalCommand = null;
            switch (command)
            {
                case CommandConstants.COMMAND_LOGIN_CODE:
                    finalCommand = new LoginCommand(logSender);
                    break;
                case CommandConstants.COMMAND_GET_GAMES_CODE:
                    finalCommand = new GetGamesCommand(logSender);
                    break;
                case CommandConstants.COMMAND_SEARCH_GAMES_CODE:
                    finalCommand = new SearchGamesCommand(logSender);
                    break;
                case CommandConstants.COMMAND_EXIT_CODE:
                    finalCommand = new ExitCommand(logSender);
                    break;
                case CommandConstants.COMMAND_PUBLISH_GAME_CODE:
                    finalCommand = new PublishGameCommand(logSender);
                    break;
                case CommandConstants.COMMAND_PUBLISH_REVIEW_CODE:
                    finalCommand = new PublishReviewCommand(logSender);
                    break;
                case CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE:
                    finalCommand = new CheckGameOwnerCommand(logSender);
                    break;
                case CommandConstants.COMMAND_DELETE_GAME_CODE:
                    finalCommand = new DeleteGameCommand(logSender);
                    break;
                case CommandConstants.COMMAND_SELECT_GAME_CODE:
                    finalCommand = new SelectGameCommand(logSender);
                    break;
                case CommandConstants.COMMAND_ACQUIRE_GAME_CODE:
                    finalCommand = new AcquireGameCommand(logSender);
                    break;
                case CommandConstants.COMMAND_MODIFY_GAME_CODE:
                    finalCommand = new ModifyGameCommand(logSender);
                    break;
                case CommandConstants.COMMAND_GET_GAME_SCORE_CODE:
                    finalCommand = new GetGameScoreCommand(logSender);
                    break;
                case CommandConstants.COMMAND_VIEW_REVIEW_CODE:
                    finalCommand = new ViewReviewCommand(logSender);
                    break;
                case CommandConstants.COMMAND_VIEW_DETAILS_CODE:
                    finalCommand = new ViewDetailsCommand(logSender);
                    break;
                case CommandConstants.COMMAND_DOWNLOAD_COVER_CODE:
                    finalCommand = new DownloadCoverCommand(logSender);
                    break;
                default:
                    Console.WriteLine("Command doesnt exist");
                    break;
            }

            return finalCommand;
        }
    }
}