using System;
using ConsoleMenusInterfaces;
using ServerApplicationInterfaces;

namespace ConsoleMenus.Server
{
    // Deprecated makinola
    public class ConsoleStartServerMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            /*
            bool serverStarted = IServerHandler.Instance.StartServer();

            if(serverStarted)
            {
                _nextMenu = new ConsoleListeningForClientsMenu();
            }else
            {
                _nextMenu = this;
            }
            
            */
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Starting up server...");
        }
    }
}
