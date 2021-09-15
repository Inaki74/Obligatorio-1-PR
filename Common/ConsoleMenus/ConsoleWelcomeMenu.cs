
using Common.ConsoleMenus.Interfaces;
using Common.ConsoleMenus.Factory;
using System;

namespace Common.ConsoleMenus
{
    public class ConsoleWelcomeMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public void Action(string answer)
        {
            // Delegar a ConnectMenu
            _nextMenu = _factory.Create<ConsoleConnectMenu>();
        }

        public void PrintMenu()
        {
            Console.WriteLine("Welcome to the VAPOR System!");
        }
    }
}