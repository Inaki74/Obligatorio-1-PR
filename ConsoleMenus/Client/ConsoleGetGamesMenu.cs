using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleGetGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            // Conseguir la lista
            

            // Imprimirla

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Fetching all games ...");
        }
    }
}
