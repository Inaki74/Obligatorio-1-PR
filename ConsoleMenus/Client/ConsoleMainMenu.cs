
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;
using Common.Protocol;

namespace ConsoleMenus.Client
{
    public class ConsoleMainMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string EXIT_OPTION = "0";
        private const string PUBLISH_GAME_OPTION = "1";
        private const string GET_ALL_GAMES_OPTION = "2";
        private const string GET_GAMES_OPTION = "4";

        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            switch(answer)
            {
                case EXIT_OPTION:
                    string response = IClientHandler.Instance.Exit();
                    Console.WriteLine(response);
                    return true;
                case PUBLISH_GAME_OPTION:
                    _nextMenu = new ConsolePublishGameMenu();
                    break;
                case GET_ALL_GAMES_OPTION:
                    _nextMenu = new ConsoleGetGamesMenu();
                    break;
                case GET_GAMES_OPTION:
                    _nextMenu = new ConsoleSearchGamesMenu();
                    break;
                default:
                    Console.WriteLine("Not a valid option.");
                    _nextMenu = this;
                    break;
            }

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Select an option: ");
            Console.WriteLine($"{PUBLISH_GAME_OPTION}. Publish game.");
            Console.WriteLine($"{GET_ALL_GAMES_OPTION}. Get list of all games.");
            Console.WriteLine($"{GET_GAMES_OPTION}. Search game.");
            Console.WriteLine($"{EXIT_OPTION}. Exit application.");
        }
    }
}