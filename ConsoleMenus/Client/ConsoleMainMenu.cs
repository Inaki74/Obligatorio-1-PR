
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


        public void Action(string answer)
        {
            switch(answer)
            {
                case EXIT_OPTION:
                    IClientHandler.Instance.Exit();
                    break;
                default:
                    Console.WriteLine("Not a valid option.");
                    _nextMenu = this;
                    break;
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("Select an option: ");
            Console.WriteLine("0. Exit application.");
        }
    }
}