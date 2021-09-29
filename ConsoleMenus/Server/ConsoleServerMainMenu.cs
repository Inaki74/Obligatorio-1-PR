using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleServerMainMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string EXIT_OPTION = "1";
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            switch(answer)
            {
                case EXIT_OPTION:
                    //IServerHandler.Instance.CloseServer();
                    break;
                default:
                    Console.WriteLine("Thats not a valid option.");
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