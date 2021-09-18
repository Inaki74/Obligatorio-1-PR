using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    public class ConsoleClientFoundMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public void Action(string answer)
        {
            IServerHandler.Instance.StartClientThread();
            
            _nextMenu = new ConsoleListeningForClientsMenu();
        }

        public void PrintMenu()
        {
            Console.WriteLine("Client found!");
        }
    }
}