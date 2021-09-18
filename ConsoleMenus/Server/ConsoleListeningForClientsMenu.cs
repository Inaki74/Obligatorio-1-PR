using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleListeningForClientsMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public void Action(string answer)
        {
            IServerHandler.Instance.ListenForClients();
            
            _nextMenu = new ConsoleClientFoundMenu();
        }

        public void PrintMenu()
        {
            Console.WriteLine("Listening for clients...");
        }
    }
}
