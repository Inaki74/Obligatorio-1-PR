using System;
using ConsoleMenus.Client;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleServerMainMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string EXIT_OPTION = "1";
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            switch(answer)
            {
                case EXIT_OPTION:
                    IServerHandler.Instance.CloseServer();
                    _nextMenu = new ConsoleExitMenu();
                    return true;
                default:
                    Console.WriteLine("Thats not a valid option.");
                    _nextMenu = this;
                    break;
            }

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Welcome to the VAPOR server!");
            Console.WriteLine($"{EXIT_OPTION}. Exit. Close server and all connections.");
        }
    }
}