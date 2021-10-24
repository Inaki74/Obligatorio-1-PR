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
            IServerHandler.Instance.StartClientListeningTask();
            
            _nextMenu = new ConsoleServerMainMenu();

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Starting client listening thread...");
        }
    }
}
