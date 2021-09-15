
using Common.ConsoleMenus.Interfaces;
using System;

namespace Common.ConsoleMenus
{
    public class ConsoleConnectMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public void Action(string answer)
        {
            // Conectar
            // Si la conexion es buena
                // Decir que fue buena y delegar a Login
            // Si no
                // Decir que fue mala e intentar de nuevo
            
        }

        public void PrintMenu()
        {
            Console.WriteLine("Connecting...");
        }
    }
}