using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleListeningForClientsMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            IServerHandler.Instance.ListenForClients();
            
            _nextMenu = new ConsoleClientFoundMenu();

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Listening for clients...");
        }
    }
}
