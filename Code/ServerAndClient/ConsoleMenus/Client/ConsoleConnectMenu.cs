
using ConsoleMenusInterfaces;
using ClientApplicationInterfaces;
using System;

namespace ConsoleMenus.Client
{
    public class ConsoleConnectMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            bool connectedSuccess = IClientHandler.Instance.ConnectToServerAsync().Result;

            if(connectedSuccess)
            {
                Console.WriteLine("Connection to server successful!");
                _nextMenu = new ConsoleLoginMenu();
            }
            else
            {
                Console.WriteLine("Couldn't connect to server, trying again...");
                _nextMenu = this;
            }
            
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Trying to connect to server...");
        }
    }
}