using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleStartServerMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            IServerHandler.Instance.StartServer();

            _nextMenu = new ConsoleListeningForClientsMenu();

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Starting up server...");
        }
    }
}
