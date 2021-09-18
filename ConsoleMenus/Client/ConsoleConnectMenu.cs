
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
            // Conectar
            // Si la conexion es buena
                // Decir que fue buena y delegar a Login
            // Si no
                // Decir que fue mala e intentar de nuevo
            bool connectedSuccess = IClientHandler.Instance.ConnectToServer();

            if(connectedSuccess)
            {
                Console.WriteLine("Connection to server successful!");
                _nextMenu = new ConsoleLoginMenu();
                return false;
            }

            Console.WriteLine("Couldn't connect to server, trying again...");
            _nextMenu = this;
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Trying to connect to server...");
        }
    }
}