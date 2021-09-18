
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleMainMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string EXIT_OPTION = "0";

        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;


        public bool Action(string answer)
        {
            switch(answer)
            {
                case EXIT_OPTION:
                    IClientHandler.Instance.Exit();
                    return true;
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
            Console.WriteLine("0. Exit application.");
        }
    }
}