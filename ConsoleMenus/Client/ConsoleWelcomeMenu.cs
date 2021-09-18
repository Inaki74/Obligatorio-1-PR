
using ConsoleMenusInterfaces;
using System;

namespace ConsoleMenus.Client
{
    public class ConsoleWelcomeMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            // Delegar a ConnectMenu
            _nextMenu = new ConsoleConnectMenu();
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Welcome to the VAPOR System!");
        }
    }
}