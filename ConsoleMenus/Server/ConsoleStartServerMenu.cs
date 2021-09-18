using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleStartServerMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public void Action(string answer)
        {
            IServerHandler.Instance.StartServer();

            _nextMenu = new ConsoleListeningForClientsMenu();
        }

        public void PrintMenu()
        {
            Console.WriteLine("Starting up server...");
        }
    }
}
